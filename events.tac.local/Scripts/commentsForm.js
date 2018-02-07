function createCommentItem(form, path) {
    var service = new ItemService({ url: '/sitecore/api/ssc/item' });
    var obj = {
        ItemName: 'comment - ' + form.email.value,
        TemplateID: '{6F806945-121E-49CA-B290-AB81E2DD552C}',
        Name: form.email.value,
        Comment: form.comment.value
    }
    service.create(obj)
    .path(path)
    .execute()
    .then(function (item) {
        form.email.value = form.comment.value = '';
        window.alert("Thanks. Your spammy message will appear on the site shortly");
    })
    .fail(function (err) { window.alert(err); });
    event.preventDefault();
    return false;
}