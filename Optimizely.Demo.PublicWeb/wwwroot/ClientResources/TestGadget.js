define([
    // Dojo
    //"dojo",
    "dojo/_base/declare",
    "dojo/html",
    // Dijit
    "dijit/_TemplatedMixin",
    "dijit/_WidgetBase",
    //CMS
    //"epi/dependency",
    //"epi/_Module",
    //"epi/shell/command/_Command",
    "epi-cms/_ContentContextMixin",
    "epi/dependency",
    "/ClientResources/MyGlobalToolbarProvider.js"
], function (
    // Dojo
    //dojo,
    declare,
    //dependency,
    html,
    //_Module,
    //_Command,
    //projectPluginArea,
    // Dijit
    _TemplatedMixin,
    _WidgetBase,
    //CMS
    _ContentContextMixin,
    dependency,
    MyGlobalToolbarProvider
) {
    return declare([_WidgetBase, _TemplatedMixin, _ContentContextMixin], {
        // summary: A simple widget that listens to changes to the
        // current content item and puts the name in a div.

        //initialize: function () {
        //    this.inherited(arguments);

        //    // adding command to project plugin area
        //    var commandregistry = dependency.resolve("epi.globalcommandregistry");
        //    //projectPluginArea.add(ProjectItemCommand);
        //    //projectPluginArea.add(ProjectCommand);
        //},

        templateString: '<div>\
                            Project name:\
                            <div data-dojo-attach-point="contentName"></div>\
                         </div>',

        postMixInProperties: function () {
            //this.getCurrentContent().then(function (context) {
            //    this._updateUI(context);
            //}.bind(this));
        },

        contextChanged: function (context, callerData) {
            this.inherited(arguments);

            // the context changed, probably because we navigated or published something
            this._updateUI(context);
        },

        _updateUI: function (context) {
            if (context != null && context.type == 'epi.cms.project') {
                html.set(this.contentName, '<a href=/AdminPlugin/project?id=' + context.id + '>' + context.name + '</a>');
            }
            else {
                if (this.contentName != null) { 
                    html.set(this.contentName, 'Not a project!');
                }
            }
        },

        _addCommands: function () {
            var commandregistry = dependency.resolve("epi.globalcommandregistry");
            commandregistry.registerProvider("epi.cms.publishmenu", new MyGlobalToolbarProvider());

            var projectService = dependency.resolve("epi.cms.ProjectService");
        },

        startup: function () {
            this._updateUI(this._currentContext);
            this._addCommands();
        },

        //_execute: function () {
        //    var registry = dependency.resolve("epi.storeregistry");
        //    this.store = this.store || registry.get("workspace.settingstore");
        //    dojo.when(this.store.get(),
        //        function (item) {
        //            topic.publish("/epi/shell/context/request",
        //                { uri: "epi.cms.contentdata:///" + item.pageRef },
        //                { sender: this, typeIdentifiers: this.typeIdentifiers });
        //        });
        //}
    });
});
