axios({
    method: "get",
    url: "api/Login"
}).then(
    res => {
        console.log(res)
        console.log(res.data)
        if (res.data == "尚未登入") {
            $(location).attr("href", "login.html")
        }
    }
).catch(
    err => {
        console.log(err)
    }
)