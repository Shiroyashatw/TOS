const { ref, reactive, onBeforeMount } = Vue;
    const url = "/api/Exchange";
    const login = "/api/User/UserName";
    const App = {
      setup() {
        const data = reactive({
          carddata: "",
          smartdata: "",
          userName: "",
        });
        onBeforeMount(() => {
          axios
            .get(url)
            .then((res) => {
              //console.log(res.data);
              data.carddata = res.data;
            })
            .catch((err) => {
              console.log(err);
            });
            // 抓取玩家名稱 導覽列進行更新
            axios.get(login).then(res => {
              //console.log(res.data)
              data.userName = res.data
            }).catch(err => {
              console.log(err)
            })
        });
        const getSmartData = () => {
          //alert('123')
          axios({
            method: 'get',
            url: '/api/Exchange/Can'
          }).then(res => {
            //console.log(res.data)
            $('#All').empty()
            data.smartdata = res.data
            //console.log((data.smartdata).length)
            if((data.smartdata).length < 1){
              alert("尚無配對成功的列表")
            }
            else{
              alert(`搜尋成功共`+(data.smartdata).length+"筆")
            }
          }).catch(err => {
            if(err.response.data == "尚未設定卡片") {
              alert("請先點選 導覽列-我的列表設定資訊")
            }
            //console.log(err)
          })
        }
        const getAllData = () => {
          axios
            .get(url)
            .then((res) => {
              $('#smart').empty();
              //data.carddata = res.data
            })
            .catch((err) => {
              console.log(err);
            });
        }
        return { data, getSmartData,getAllData };
      },
    };

    //console.log(data)
    const myVue = Vue.createApp(App).mount("#app");