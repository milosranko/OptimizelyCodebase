using EPiServer.Shell.Navigation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Optimizely.Demo.PublicWeb.Gadgets;

[MenuProvider]
public class AdminPlugin : IMenuProvider
{
	public IEnumerable<MenuItem> GetMenuItems()
	{
		var urlMenuItem1 = new UrlMenuItem("Translator", MenuPaths.Global + "/cms/adminplugin", "/episerver/adminplugin")
		{
			IsAvailable = context => true,
			SortIndex = 100
		};

		var urlMenuItem1_1 = new UrlMenuItem("Dashboard", MenuPaths.Global + "/cms/adminplugin/index", "/episerver/adminplugin")
		{
			IsAvailable = context => true,
			SortIndex = 110
		};

		var urlMenuItem1_2 = new UrlMenuItem("Projects", MenuPaths.Global + "/cms/adminplugin/projects", "/episerver/adminplugin/projects")
		{
			IsAvailable = context => true,
			SortIndex = 120
		};

		var urlMenuItem1_3 = new UrlMenuItem("Settings", MenuPaths.Global + "/cms/adminplugin/settings", "/episerver/adminplugin/settings")
		{
			IsAvailable = context => true,
			SortIndex = 130
		};

		var urlMenuItem1_4 = new UrlMenuItem("Project", MenuPaths.Global + "/cms/adminplugin/project", "/episerver/adminplugin/project")
		{
			IsAvailable = context =>
			{
				return context.Request.Headers["Referer"].Count() > 0 &&
					   context.Request.Headers["Referer"][0].Contains($"{nameof(Project)}?id=", StringComparison.InvariantCultureIgnoreCase);
			},
			SortIndex = 125
		};

		return new List<MenuItem>()
		{
			urlMenuItem1,
			urlMenuItem1_1,
			urlMenuItem1_2,
			urlMenuItem1_3,
			urlMenuItem1_4
		};
	}
}

[Authorize(Roles = "CmsAdmins,CmsEditors,WebAdmins,Administrators")]
[Route("episerver/[controller]")]
public class AdminPluginController : Controller
{
	private readonly ProjectRepository _projectRepository;

	public AdminPluginController(ProjectRepository projectRepository)
	{
		_projectRepository = projectRepository;
	}

	public IActionResult Index()
	{
		return View("/Views/Plugins/AdminPlugin/Index.cshtml");
	}

	[Route("projects")]
	public IActionResult Projects()
	{
		var model = _projectRepository.List();
		return View("/Views/Plugins/AdminPlugin/Projects.cshtml", model);
	}

	[Route("project")]
	public IActionResult Project(int id)
	{
		if (id < 1)
			return RedirectToAction(nameof(Index));

		var project = _projectRepository.Get(id);

		if (project is null)
			return RedirectToAction(nameof(Index));

		//TODO Check if there is already created translation project with this ID
		//If it exists show project details
		//If it doesn't exists show dialog for creating a new one

		var model = new ProjectViewModel
		{
			Id = project.ID,
			Name = project.Name,
			Status = project.Status,
			Items = _projectRepository.ListItems(id)
		};

		return View("/Views/Plugins/AdminPlugin/Project.cshtml", model);
	}

	[Route("settings")]
	public IActionResult Settings()
	{
		return View("/Views/Plugins/AdminPlugin/Index.cshtml");
	}
}

public class ProjectViewModel
{
	public int Id { get; set; }
	public string Name { get; set; }
	public ProjectStatus Status { get; set; }
	public IEnumerable<ProjectItem> Items { get; set; }
}

//[ServiceConfiguration(typeof(ViewConfiguration))]
//public class MyView : ViewConfiguration<IContentData>
//{
//    public MyView()
//    {
//        Key = "my-view";
//        Name = "React View";
//        ControllerType = "alloy/components/ReactGadget";
//        IconClass = "epi-iconStar";
//        SortOrder = 100;
//    }
//}
