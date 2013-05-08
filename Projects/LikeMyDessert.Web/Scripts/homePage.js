var home_page = {
    $add_button: $("#add-button"),
    $updateButton: $("#update-button"),
    $dessertPane: $("#desserts-container"),
    loadDessertPane: function () {
        home_page.$dessertPane.load("/Home/UpdateDessertPane");
    }
};

$(function () {
    home_page.$updateButton.live("click", home_page.loadDessertPane);
});