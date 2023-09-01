var ModelDrivenAppCharacterEvents = window.ModelDrivenAppCharacterEvents || {};

(function () {

    // Code to run in the form OnLoad event
    this.formOnLoad = function (executionContext) {
        console.log("ModelDrivenAppCharacterEvents - formOnLoad");

        // Current version of the file
        console.log("version", "2023.08.01.1");

        var formContext = executionContext.getFormContext();
    }

    // Code to run in the column OnChange event 
    this.attributeOnChange = function (executionContext) {
        console.log("ModelDrivenAppCharacterEvents - attributeOnChange");

        var formContext = executionContext.getFormContext();

        var characterId = formContext.getAttribute("maruma_externalprimarykey").getValue();

        if (characterId) {
            formContext.getAttribute("maruma_url").setValue("https://swapi.dev/api/people/" + characterId);
        }
    }

    // Code to run in the form OnSave event 
    this.formOnSave = function () {
        console.log("ModelDrivenAppCharacterEvents - formOnSave");
    }

}).call(ModelDrivenAppCharacterEvents);