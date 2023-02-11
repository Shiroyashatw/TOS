$('#btnSend').on('click', function () {
    const InputAccVal = $('input[name="account"]').val()
    const InputPasVal = $('input[name="password"]').val()
    const InputNameVal = $('input[name="username"]').val()

    if(InputAccVal == '' || InputPasVal == '' || InputNameVal == ''){
        alert("有欄位尚未輸入")
        return
    }
    axios({
        method: "post",
        url: 'api/Singup',
        data: {
            account: InputAccVal,
            password: InputPasVal,
            username: InputNameVal,
        }
    })
        .then(
            (res) => {
                
                if(res.data == "註冊成功"){
                    alert("註冊成功!")
                    $(location).attr("href", "login.html")
                }
                
            }
        )
        .catch((error) => {
            //console.log(error.response.data)
            if(error.response.data == "重複"){
                alert("該帳號名稱已有人使用")
            }
            
        })
})