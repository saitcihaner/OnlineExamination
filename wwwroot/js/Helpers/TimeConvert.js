function ConvertToDateTime() {
    var today = new Date();
    var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
    var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
    var dateTime = date + ' ' + time;
    return dateTime;
}

function TransformJSDatetime(_date) {
    const D = new Date(_date)
    return D.getMonth() + 1 + "/" + D.getDate() + "/" + D.getFullYear()

}
function Timestampa(_date) {
    var dateStr = _date
    //returned from mysql timestamp/datetime field
    var a = dateStr.split(" ");
    var d = a[0].split("-");
    var t = a[1].split(":");
    var formatedDate = new Date(d[0], (d[1] - 1), d[2], t[0], t[1], t[2]);
}