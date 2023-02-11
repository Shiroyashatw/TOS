$('#btnSend').on('click', function () {
    const InputAccVal = $('input[name="account"]').val()
    const InputPasVal = $('input[name="password"]').val()

    if(InputAccVal == '' || InputPasVal == ''){
        alert("有欄位尚未輸入")
        return
    }
    axios({
        method: "post",
        url: "api/Login",
        data: {
            account: InputAccVal,
            password: InputPasVal,
        }
    }).then(
        res => {
            if(res.data == "帳號密碼錯誤"){
                alert("帳號密碼錯誤!")
            }
            if (res.data == "OK") {
                $(location).attr("href", "Change.html")
            }

        }

    ).catch((error) => {
        
    }
    )
})