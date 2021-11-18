function WhichButtonIsClicked(sayı) {
    //Mantık şu şekilde bir attribute her buttonda farklı olan ile eşitle ben burda i ile name eşitledi. Yada ögrenci noyu func içine atıp name eşitleyebilşdim
    //O parametreyide func içine ver
    var button = $("button[name ='" + sayı + "']")
    var item = button.closest("tr")
    var ab = item.find("td:eq(0)").html()
    // Finds the closest row <tr>
    alert(item.find("td:eq(0)").html())
}