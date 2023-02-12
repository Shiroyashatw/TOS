$('#LoginOut').on('click', function () {
    axios({
      method: "delete",
      url: "api/Login"
    }).then(
      res => {
        if (res.data == "登出") {
          $(location).attr("href", "login.html")
        }
      }
    ).catch(
      err => console.log(err)
    )
  })