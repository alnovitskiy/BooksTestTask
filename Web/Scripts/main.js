function show() {
    var isbnsText = $("#isbnTA").val();
    if (isbnsText.length == 0) {
        alert("Please paste ISBN numbers.");
        return false;
    }
    var ajaxUrl = 'Book/GetList?isbns=' + isbnsText;
    $.get(ajaxUrl, function (result) { $('#books').html(result); });
    return false;
}

function bookMark(checkEl, isbn) {
    $.ajax({
        type: 'POST',
        url: 'Book/Mark',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ isMarked : checkEl.checked, isbn : isbn }),
        async: true,
        success: function (result) {
            if (result.error.length > 0)
                alert("Books marking complited with an error: " + result.error);
        },
        error: function (error) {
            alert("Books marking complited with an error: " + error);
        }
    });

}