const { ref, reactive } = Vue;
        const url = '/api/Forget'
        const App = {
            setup() {
                const data = reactive({
                    checkVal: '',
                    username: ''
                })
                const CheckUser = () => {
                    let InputAccVal = $('input[name="account"]').val()
                    let InputNameVal = $('input[name="username"]').val()
                    if (InputAccVal == '' || InputNameVal == '') {
                        alert("有欄位尚未輸入")
                        return
                    }
                    axios({
                        method: 'post',
                        url: url,
                        data: {
                            account: InputAccVal,
                            username: InputNameVal,
                        }
                    }).then(res => {
                        data.checkVal = res.data
                        data.username = InputNameVal
                        if (res.data == "無資料") {
                            alert("查無對應帳號名稱")
                        }
                    }).catch(err => {
                        console.log(err)
                    })
                }
                const CheckPass = () => {
                    let InputPasVal = $('input[name="password"]').val()
                    let InputCkPasVal = $('input[name="Checkpassword"]').val()
                    if (InputPasVal == '' || InputCkPasVal == '') {
                        alert("有欄位尚未輸入")
                        return
                    }
                    if (InputPasVal != InputCkPasVal) {
                        alert("密碼確認錯誤")
                        return
                    }
                    axios({
                        method: 'patch',
                        url: url,
                        data: {
                            password: InputPasVal,
                            username: data.username,
                        }
                    }).then(res => {
                        //console.log(res.data)
                        if (res.data == "Ok") {
                            alert("密碼重設成功!")
                            $(location).attr("href", "login.html")
                        }
                    }).catch(err => {
                        console.log(err)
                    })
                }
                return { data, CheckUser, CheckPass };
            },
        };


        //console.log(data)
        const myVue = Vue.createApp(App).mount("#app")