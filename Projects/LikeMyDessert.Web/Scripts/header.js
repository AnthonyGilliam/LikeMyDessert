var header = {
	$navLink: $('header nav a'),

	OpenMenuItem: function (menuItem) {
		//TODO: Add AJAX function for loading menu item page here.

		var liText = menuItem.text();
		alert('You clicked on ' + liText + '!');
	}
};

//Initialize
$(function () {
    header.$navLink.live('click', function (e) {
        e.preventDefault();
        $.ajax({
            url: $(this).attr('href'),
            success: function (results) { $("#main").html(results); },
            error: function (jqXHR, textStatus, errorThrown) { alert(errorThrown); }
        });
    });
});