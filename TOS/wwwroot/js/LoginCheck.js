axios({
    method: "get",
    url: "api/Login"
}).then(
    res => {
        //console.log(res)
        if (res.data == "已登入") {
            $(location).attr("href", "Change.html")
        }
    }
).catch(
    err => {
        console.log(err)
    }
)