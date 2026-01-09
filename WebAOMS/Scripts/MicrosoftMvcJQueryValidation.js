var validationSummaryId = validationContext.ValidationSummaryId;
if (validationSummaryId) {
    // insert an empty <ul> into the validation summary <div> tag (as necessary)
    $("<ul />").appendTo($("#" + validationSummaryId + ":not(:has(ul:first))"));

    options = {
        errorContainer: "#" + validationSummaryId,
        errorLabelContainer: "#" + validationSummaryId + " ul:first",
        wrapper: "li",

        showErrors: function (errorMap, errorList) {
            var errContainer = $(this.settings.errorContainer);
            var errLabelContainer = $("ul:first", errContainer);

            // Add error CSS class to user-input controls with errors
            for (var i = 0; this.errorList[i]; i++) {
                var element = this.errorList[i].element;
                var messageSpan = $(fieldToMessageMappings[element.name]);
                var msgSpanHtml = messageSpan.html();
                if (!msgSpanHtml || msgSpanHtml.length == 0) {
                    // Don't override the existing Validation Message.
                    // Only if it is empty, set it to an asterisk.
                    messageSpan.html("");
                }
                messageSpan.removeClass("field-validation-valid").addClass("field-validation-error");
                $("#" + element.id).addClass("input-validation-error");
            }
            for (var i = 0; this.successList[i]; i++) {
                // Remove error CSS class from user-input controls with zero validation errors
                var element = this.successList[i];
                var messageSpan = fieldToMessageMappings[element.name];
                $(messageSpan).addClass("field-validation-valid").removeClass("field-validation-error");
                $("#" + element.id).removeClass("input-validation-error");
            }

            if (this.numberOfInvalids() > 0) {
                errContainer.removeClass("validation-summary-valid").addClass("validation-summary-errors");
            }

            this.defaultShowErrors();

            // when server-side errors still exist in the Validation Summary, don't hide it
            var totalErrorCount = errLabelContainer.children("li:not(:has(label))").length + this.numberOfInvalids();
            if (totalErrorCount > 0) {
                $(this.settings.errorContainer).css("display", "block").addClass("validation-summary-errors").removeClass("validation-summary-valid");
                $(this.settings.errorLabelContainer).css("display", "block");
            }
        },
        messages: errorMessagesObj,
        rules: rulesObj
    };
}