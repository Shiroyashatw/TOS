const { ref, reactive, onBeforeMount } = Vue;
        // 讀取當前網址 
        const getUrlString = location.href;
        const Nowurl = new URL(getUrlString);

        // 根據前面網址抓取 撈後面參數資料 UserId 進行設定
        const UserId = Nowurl.searchParams.get('id');
        const url = `/api/User/` + UserId
        const login = "/api/User/UserName";



        const App = {
            setup() {
                const data = reactive({
                    carddata: '',
                    BackupState: '',
                    userName: "",
                })
                onBeforeMount(() => {
                    axios.get(url).then(
                        res => {
                            //console.log(res.data)
                            data.carddata = res.data
                        }
                    ).catch(err => {
                        console.log(err)
                    })

                    axios({
                        method: "get",
                        url: "api/User",
                    }).then(res => {
                        //console.log(res)
                        data.BackupState = res.data;
                    }).catch(err => {
                        console.log(err)
                    })

                    axios.get(login).then(res => {
                        //console.log(res.data)
                        data.userName = res.data
                    }).catch(err => {
                        console.log(err)
                    })
                })
                return { data };
            },
        };


        //console.log(data)
        const myVue = Vue.createApp(App).mount("#app")