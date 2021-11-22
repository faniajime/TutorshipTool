scheduler.customConfiguration = function () {
    scheduler.attachEvent("onEventSave", function (id, ev, is_new) {
        if (!ev.text) {
            dhtmlx.alert("Text must not be empty");
            return false;
        }

        return true;
    });
};