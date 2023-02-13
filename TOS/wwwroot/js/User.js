const { ref, reactive, onBeforeMount } = Vue;
        const url = '/api/CardList'
        const login = "/api/User/UserName";
        const App = {
            setup() {
                const data = reactive({
                    carddata: '',
                    BackupState: '',
                    userdata: '',
                    userName: "",
                })
                const group = ref([])
                const wantgroup = ref([])
                const max = 5
                const UpdateMax = 2
                onBeforeMount(() => {
                    axios.get(url).then(
                        res => {
                            //console.log(res.data)
                            data.carddata = res.data
                        }
                    ).catch(err => {
                        console.log(err)
                    })
                    // 確認是否已新增過資料
                    axios({
                        method: "get",
                        url: "api/User",
                    }).then(res => {
                        //console.log(res)
                        data.BackupState = res.data;
                    }).catch(err => {
                        console.log(err)
                    })

                    // 抓取玩家名稱 導覽列進行更新
                    axios.get(login).then(res => {
                        //console.log(res.data)
                        data.userName = res.data
                    }).catch(err => {
                        console.log(err)
                    })

                    // 新增過則取出列表
                    axios({
                        method: "get",
                        url: "api/User/Info",
                    }).then(res => {
                        //console.log(res)
                        data.userdata = res.data;
                    }).catch(err => {
                        console.log(err)
                    })
                })
                const PostData = () => {
                    let BackupVal = $('input[name="BackupState"]:checked').val()
                    let accountInfoVal = $('textarea#InfoText').val();
                    let resArray = [];
                    let wantresArray = [];

                    $('input:checkbox[name=card]:checked').each(function () {
                        let res = $(this).val()
                        resArray.push(res)

                        // add $(this).val() to your array
                    });
                    $('input:checkbox[name=wantcard]:checked').each(function () {
                        let wantres = $(this).val()
                        wantresArray.push(wantres)

                        // add $(this).val() to your array
                    });
                    if (BackupVal == null) {
                        alert("請選擇帳號狀態!")
                        return
                    }
                    if (accountInfoVal == '') {
                        alert("請輸入聯絡管道!")
                        return
                    }
                    if (resArray.length != 5) {
                        alert("請選擇五張一抽抽中卡片")
                        return
                    }
                    if (wantresArray.length != 5) {
                        alert("請選擇五張想交換的卡片")
                        return
                    }
                    if (BackupVal == "true") {
                        BackupVal = true
                    }
                    else {
                        BackupVal = false
                    }
                    //console.log(resArray)
                    //console.log(wantresArray)
                    //console.log(accountInfoVal)
                    axios({
                        method: 'post',
                        url: '/api/User',
                        data: {
                            BackupState: BackupVal,
                            HaveCard: resArray,
                            Wantcard: wantresArray,
                            AccountInfo: accountInfoVal,
                        }
                    }).then(res => {
                        alert("新增成功!")
                        $(location).attr("href", "Change.html")
                        //console.log(res)
                    }).catch(err => {
                        //console.log(err)
                    })
                }

                // 更新已被選走的
                const UpdateChangeList = () => {
                    let UpdateresArray = [];
                    $('input:checkbox[name=updatecard]:checked').each(function () {
                        let res = $(this).val()
                        UpdateresArray.push(res)
                    });
                    if (UpdateresArray.length < 1) {
                        alert("請選擇要更新的卡片")
                        return
                    }
                    axios({
                        method: 'patch',
                        url: "/api/User",
                        data: {
                            UpdateChangeList: UpdateresArray
                        }
                    }).then(res => {
                        console.log("Y")
                        alert("更新成功")
                        $(location).attr("href", "change.html")
                    }).catch(err => {
                        console.log("N")
                    })

                }
                return { data, group, wantgroup, max, UpdateMax, PostData, UpdateChangeList };
            },
        };


        //console.log(data)
        const myVue = Vue.createApp(App).mount("#app")