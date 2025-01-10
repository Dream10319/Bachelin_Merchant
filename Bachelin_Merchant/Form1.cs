using Amazon.S3.Transfer;
using MySql.Data.MySqlClient.Memcached;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using RestSharp;

namespace Bachelin_Merchant
{
    public partial class Form1 : Form
    {
        SplashThread splash;

        string bucketName = "baechelin";
        string accessKeyId = "";
        string secretAccessKey = "";

        private string strConnMy_read = "";
        private string strConnMy = "";

        SQLiteConnection sqlite_conn;

        public SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source = Store.db; Version = 3; New = True; Compress = True;");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {

            }
            return sqlite_conn;
        }

        public void CreateTable(SQLiteConnection conn)
        {

            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE IF NOT EXISTS StoreTable (bm_shopnm VARCHAR(255), bm_telno VARCHAR(255), bm_address VARCHAR(255), bm_cate VARCHAR(255), bm_shopcd VARCHAR(255), lat VARCHAR(255), lng VARCHAR(255), logoUrl VARCHAR(255))";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();

        }

        public void ClearTable(SQLiteConnection conn)
        {

            SQLiteCommand sqlite_cmd;
            string Deletesql = "DELETE FROM StoreTable;";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Deletesql;
            sqlite_cmd.ExecuteNonQuery();

        }

        public void InsertData(SQLiteConnection conn, object[] data)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = string.Format("INSERT INTO StoreTable (bm_shopnm, bm_telno, bm_address, bm_cate, bm_shopcd, lat, lng, logoUrl) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');", data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7]);
            sqlite_cmd.ExecuteNonQuery();
        }

        public void ReadData(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM StoreTable";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                object[] data1 = new object[8];
                data1[0] = sqlite_datareader.GetString(0);
                data1[1] = sqlite_datareader.GetString(1);
                data1[2] = sqlite_datareader.GetString(2);
                data1[3] = sqlite_datareader.GetString(3);
                data1[4] = sqlite_datareader.GetString(4);
                data1[5] = sqlite_datareader.GetString(5);
                data1[6] = sqlite_datareader.GetString(6);
                data1[7] = sqlite_datareader.GetString(7);

                dataGridView3.Rows.Add(data1);
            }
        }

        #region -생성자
        public Form1()
        {
            InitializeComponent();
            dataGridView3.CellDoubleClick += DataGridView3_CellDoubleClick;
            /*if (File.Exists("Store.db"))
            {
                sqlite_conn = CreateConnection();
                ReadData(sqlite_conn);
                MessageBox.Show("Successfully loaded from database");
            }
            else
            {
                sqlite_conn = CreateConnection();
                CreateTable(sqlite_conn);
            }*/
        }

        private void DataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            object[] data1 = new object[8];
            data1[0] = dataGridView3.CurrentRow.Cells[0].Value;
            data1[1] = dataGridView3.CurrentRow.Cells[1].Value;
            data1[2] = dataGridView3.CurrentRow.Cells[2].Value;
            data1[3] = dataGridView3.CurrentRow.Cells[3].Value;
            data1[4] = dataGridView3.CurrentRow.Cells[4].Value;
            data1[5] = dataGridView3.CurrentRow.Cells[5].Value;
            data1[6] = dataGridView3.CurrentRow.Cells[6].Value;
            data1[7] = dataGridView3.CurrentRow.Cells[7].Value;
        }
        #endregion

        #region -파일업로드(aws)
        void SaveImageAws(string shop_cd, string org_file_nm, string save_file_nm)
        {
            SScraping _sc = new SScraping();
            CookieCollection cookieCollection = new CookieCollection();
            CookieContainer cookieContainer = new CookieContainer();
            try
            {
                byte[] bmpStream = _sc.ScrapData_Bin(org_file_nm, "", "", "GET", ref cookieCollection, ref cookieContainer);

                Stream stream = new MemoryStream(bmpStream);

                TransferUtility fileTransferUtility = new TransferUtility(accessKeyId, secretAccessKey, Amazon.RegionEndpoint.APNortheast2);
                TransferUtilityUploadRequest request = new TransferUtilityUploadRequest()
                {
                    BucketName = string.Format(@"{0}/media/shop/{1}", bucketName, shop_cd),
                    Key = save_file_nm,
                    InputStream = stream,
                };
                fileTransferUtility.Upload(request);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region -html코드 삭제
        private string RemoveHtmlCode(string HtmlSource, bool RemoveCarriageReturn)
        {
            if (RemoveCarriageReturn)
            {
                HtmlSource = HtmlSource.Replace("\n", "");
                HtmlSource = HtmlSource.Replace("\r", "");
                HtmlSource = HtmlSource.Replace("\t", "");
                HtmlSource = HtmlSource.Replace("&nbsp;", "");
            }
            return Regex.Replace(HtmlSource, @"<(.|\n)*?>", string.Empty);
        }
        #endregion

        #region -입점신청 조회
        private string GetShopDatareqdata(string shop_bizcd)
        {
            string returndata = "";
           // MySql.Data.MySqlClient.MySqlConnection myConn = new MySql.Data.MySqlClient.MySqlConnection(strConnMy_read);
         //   if (myConn.State == ConnectionState.Closed)
           // {
           //     myConn.Open();
           // }

            try
            {
                //string strqry = "SELECT * FROM TB_SHOP_REGIST_REQ a INNER JOIN TB_BIZ_ACCOUNT b ON a.biz_cd = b.biz_cd WHERE b.biz_isowner = 1 AND a.req_bizno = '" + shop_bizcd + "';";
                //string strqry = "SELECT * FROM TB_SHOP_REGIST_REQ a INNER JOIN TB_BIZ_ACCOUNT b ON a.biz_cd = b.biz_cd WHERE a.req_bizno = '" + shop_bizcd + "' AND IFNULL(a.shop_cd,0) = 0;";
                //MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(strqry, myConn);
                //MySql.Data.MySqlClient.MySqlDataReader rdr = cmd.ExecuteReader();

                //if (rdr == null)
                //{
                 //   returndata = "";
                //}
                //else
                //{
                    int i = 0;
                    //while (rdr.Read())
                    //{
                        //returndata = rdr[0].ToString();

                        object[] data1 = new object[14];
                data1[0] = "15191";// rdr["req_cd"].ToString();
                data1[1] = "분식제작소";//rdr["req_nm"].ToString();
                data1[2] = "2782701818";//rdr["req_bizno"].ToString();
                data1[3] = "울산광역시 남구 신복로62번길 16-1";//rdr["req_add_doro"].ToString();
                data1[4] = "249296";//rdr["biz_cd"].ToString();
                data1[5] = "44603";//rdr["req_zipcd"].ToString();
                data1[6] = "249354";//rdr["biz_acc_cd"].ToString();
                data1[7] = "1";//rdr["new_type"].ToString();
                data1[8] = ""; //rdr["req_bankcd"].ToString();
                data1[9] = "";//rdr["req_acctno"].ToString();
                data1[10] = ""; //rdr["req_acctowner"].ToString();
                data1[11] = ""; //rdr["req_acctimg"].ToString();
                data1[12] = "울산 남구 무거동 816-27";//rdr["req_add_jibun"].ToString();
                data1[13] = "248275"; //rdr["shop_cd"] == null ? "" : rdr["shop_cd"].ToString();

                        dataGridView2.Rows.Add(data1);

                        i++;
                    //}
                //}
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                //if (myConn.State == ConnectionState.Open)
                //{
                //    myConn.Close();
                //}
            }

            return returndata;

        }
        #endregion

        #region -매장 조회
        private string GetShopData(string shop_bizcd)
        {
            string returndata = "";
      //      MySql.Data.MySqlClient.MySqlConnection myConn = new MySql.Data.MySqlClient.MySqlConnection(strConnMy_read);
         //   if (myConn.State == ConnectionState.Closed)
         //   {
         //       myConn.Open();
         //   }

            try
            {
            //    string strqry = "SELECT * FROM `TB_SHOP_LIST` WHERE shop_bizno = '" + shop_bizcd + "';";
            //    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(strqry, myConn);
             //   MySql.Data.MySqlClient.MySqlDataReader rdr = cmd.ExecuteReader();

            //    if (rdr == null)
            //    {
             //       returndata = "";
             //   }
             //   else
           //     {
                    int i = 0;
                //    while (rdr.Read())
                //    {
                   //     returndata = rdr[0].ToString();

                        object[] data1 = new object[8];
                        data1[0] = "248275";//rdr["shop_cd"].ToString();
                        data1[1] = "분식제작소";//rdr["shop_name"].ToString();
                        data1[2] = "2782701818";//rdr["shop_bizno"].ToString();
                        data1[3] = "울산광역시 남구 신복로62번길 16-1";//rdr["shop_addr_doro"].ToString();
                        data1[4] = "분식";//rdr["ct_name"].ToString();
                        data1[5] = "";//rdr["tmp_shop_cd"].ToString();
                        data1[6] = "35.55114321";//rdr["shop_lat"].ToString();
                        data1[7] = "129.25595811";//rdr["shop_lng"].ToString();

                        dataGridView1.Rows.Add(data1);

                        i++;
                   // }
              //  }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            finally
            {
             //   if (myConn.State == ConnectionState.Open)
            //    {
            //        myConn.Close();
             //   }
            }

            return returndata;

        }
        #endregion

        #region -배민 매장 조회
        private string GetBaeminShopData(string tmp_shop_cd, string shop_bizno)
        {
            string returndata = "";
            MySql.Data.MySqlClient.MySqlConnection myConn = new MySql.Data.MySqlClient.MySqlConnection(strConnMy_read);
            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();
            }

            try
            {
                string strqry = "SELECT * FROM `TB_SHOP_LIST` WHERE tmp_shop_cd = '" + tmp_shop_cd 
                    + "' AND shop_bizno = '" + shop_bizno + "' AND shop_status <> '3';";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(strqry, myConn);
                MySql.Data.MySqlClient.MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr == null)
                {
                    returndata = "";
                }
                else
                {
                    int i = 0;
                    while (rdr.Read())
                    {
                        returndata = rdr["shop_cd"].ToString();
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }

            return returndata;

        }
        #endregion

        #region -배민 매장리스트 조회
        private void GetBaeminShopList(string lat, string lng)
        {
            //실시간 배민데이터 가져오기
            SScraping _sc = new SScraping();
            CookieCollection cookieCollection = new CookieCollection();
            CookieContainer cookieContainer = new CookieContainer();

            int ntotal = 0;
            int npage = 0;
            int nlimit = 25;
            //매장리스트
            for (; ; )
            {
                string strUrl = "https://shopdp-api.baemin.com/v2/BAEMIN/shops?displayCategory=&longitude=" + lng + "&latitude=" + lat + "&sort=SORT__DEFAULT&filter=&distance=3&offset=" + (npage * nlimit).ToString() + "&limit=" + nlimit.ToString() + "&extension=&appver=10.9.1&carrier=&site=7jWXRELC2e&deviceModel=SM-G973N&dvcid=OPUD4bf43dcba41eff4a&adid=NONE&sessionId=&osver=22&oscd=2";
                //string strUrl = "https://shopdp-api.baemin.com/v3/BAEMIN_DELIVERY_HOME/shops?displayCategory=BAEMIN_DELIVERY_HOME_ALL&longitude=" + lng + "&latitude=" + lat + "&sort=SORT__DEFAULT_RECOMMEND&filter=&distance=3&offset=" + (npage * nlimit).ToString() + "&limit=" + nlimit.ToString() + "&extension=&appver=12.15.0&carrier=302780&site=7jWXRELC2e&deviceModel=CPH1823&dvcid=OPUD3ae65d495619f1bc&adid=NONE&sessionId=&osver=32&oscd=2&dongCode=28177103&zipCode=22224";
                string strReturn = _sc.GetHtmlSource(strUrl, "utf-8", "", "", ref cookieCollection, ref cookieContainer);
                JavaScriptSerializer jss2 = new JavaScriptSerializer();
                dynamic data2 = jss2.Deserialize<dynamic>(strReturn);
                string status = data2["status"] == null ? "" : data2["status"].ToString();
                if (status != "SUCCESS")
                {
                    MessageBox.Show("매장을 조회하지 못했습니다.");
                    return;
                }
                dynamic shops = data2["data"]["shops"];
                string totalCount = data2["data"]["totalCount"] == null ? "" : data2["data"]["totalCount"].ToString();

                int.TryParse(totalCount, out ntotal);
                foreach (dynamic shop_info in shops)
                {
                    //매장기본정보 조회
                    string shopNumber = shop_info["shopInfo"]["shopNumber"] == null ? "" : shop_info["shopInfo"]["shopNumber"].ToString();    //매장코드
                    string telNumber = shop_info["shopInfo"]["telNumber"] == null ? "" : shop_info["shopInfo"]["telNumber"].ToString();    //전화번호
                    string shopName = shop_info["shopInfo"]["shopName"] == null ? "" : shop_info["shopInfo"]["shopName"].ToString();    //매장명
                    string categoryNameKor = shop_info["shopInfo"]["categoryNameKor"] == null ? "" : shop_info["shopInfo"]["categoryNameKor"].ToString();    //카테고리
                    string address = shop_info["shopInfo"]["address"] == null ? "" : shop_info["shopInfo"]["address"].ToString();    //주소
                    string logoUrl = shop_info["shopInfo"]["logoUrl"] == null ? "" : shop_info["shopInfo"]["logoUrl"].ToString();    //매장이미지

                    if (logoUrl.IndexOf(".jpg") < 0)
                    {
                        logoUrl = "";
                    }

                    object[] data1 = new object[8];
                    data1[0] = shopName;
                    data1[1] = telNumber;
                    data1[2] = address;
                    data1[3] = categoryNameKor;
                    data1[4] = shopNumber;
                    data1[5] = lat;
                    data1[6] = lng;
                    data1[7] = logoUrl;

                    dataGridView3.Rows.Add(data1);
                }
                npage++;

                if (ntotal <= (npage * nlimit))
                {
                    break;
                }
            }
        }
        #endregion

        #region -키워드로 매장리스트 조회
        private void GetBaeminShopList_Keyword(string keyword, string lat, string lng)
        {
            //실시간 배민데이터 가져오기
            SScraping _sc = new SScraping();
            CookieCollection cookieCollection = new CookieCollection();
            CookieContainer cookieContainer = new CookieContainer();

            int ntotal = 0;
            int npage = 0;
            int nlimit = 25;
            //매장리스트
            for (; ; )
            {
                //매장명으로 좌표조회
                //string strUrl = "https://shopdp-api.baemin.com/v2/SEARCH/shops?keyword=" + keyword + "&filter=&sort=SORT__DEFAULT&referral=&popularKeywordRank=&kind=&offset=" + (npage * nlimit).ToString() + "&limit=" + nlimit.ToString() + "&latitude=" + lat + "&longitude=" + lng + "&extension=&appver=11.8.2&carrier=&site=7jWXRELC2e&deviceModel=SM-G973N&dvcid=OPUD4bf43dcba41eff4a&adid=NONE&sessionId=&osver=22&oscd=2";
                string strUrl = "https://search-gateway.baemin.com/v1/search?keyword=" + keyword + "&currentTab=BAEMIN_DELIVERY&referral=&entryPoint=&kind=DEFAULT&offset=" + (npage * nlimit).ToString() + "&limit=" + nlimit.ToString() + "&latitude=" + lat + "&longitude=" + lng + "&isFirstRequest=false&extension=&baeminDeliverySort=&baeminTakeoutFilter=&baeminTakeoutSort=&isBmartRegion=true&isBaeminStoreRegion=true&commerceSort=&commerceCursor=&commerceFilters=&commerceSelectedSellerId=&commerceSelectedShopId=&commerceSearchType=DEFAULT&hyperMarketSort=&hyperMarketSearchType=DEFAULT&perseusSessionId=&memberNumber=000000000000&sessionId=&carrier=&appver=12.23.0&site=7jWXRELC2e&deviceModel=CPH1823&dvcid=OPUD3ae65d495619f1bc&adid=NONE&sessionId=&osver=32&oscd=2";
                //string strReturn = _sc.GetHtmlSource(strUrl, "utf-8", "", "", ref cookieCollection, ref cookieContainer);
                string strToken = _sc.GetHtmlSource("https://baeminsolver.onrender.com/v1/baemin/solver", "utf-8", "", "", ref cookieCollection, ref cookieContainer);
                var client = new RestClient(strUrl);
                var request = new RestRequest();
                request.AddHeader("Accept-Encoding", "gzip, deflate");
                request.AddHeader("Connection", "Keep-Alive");
                request.AddHeader("Host", "search-gateway.baemin.com");
                request.AddHeader("User-Agent", "and1_12.23.0");
                request.AddHeader("USER-BAEDAL", strToken);
                string strReturn = client.ExecuteGet(request).Content;
                JavaScriptSerializer jss2 = new JavaScriptSerializer();
                dynamic data2 = jss2.Deserialize<dynamic>(strReturn);
                string status = data2["status"] == null ? "" : data2["status"].ToString();
                if (status != "SUCCESS")
                {
                    MessageBox.Show("매장을 조회하지 못했습니다.");
                    return;
                }
                dynamic shops = data2["data"]["list"][0]["result"]["shops"];
                string totalCount = data2["data"]["list"][0]["count"] == null ? "" : data2["data"]["list"][0]["count"].ToString();

                int.TryParse(totalCount, out ntotal);
                foreach (dynamic shop_info in shops)
                {
                    //매장기본정보 조회
                    string shopNumber = shop_info["shopInfo"]["shopNumber"] == null ? "" : shop_info["shopInfo"]["shopNumber"].ToString();    //매장코드
                    //string telNumber = shop_info["shopInfo"]["telNumber"] == null ? "" : shop_info["shopInfo"]["telNumber"].ToString();    //전화번호
                    string shopName = shop_info["shopInfo"]["shopName"] == null ? "" : shop_info["shopInfo"]["shopName"].ToString();    //매장명
                    string categoryNameKor = shop_info["shopInfo"]["representationMenu"] == null ? "" : shop_info["shopInfo"]["representationMenu"].ToString();    //카테고리
                    string address = shop_info["shopInfo"]["address"] == null ? "" : shop_info["shopInfo"]["address"].ToString();    //주소
                    string logoUrl = shop_info["shopInfo"]["logoUrl"] == null ? "" : shop_info["shopInfo"]["logoUrl"].ToString();    //매장이미지

                    if (logoUrl.IndexOf(".jpg") < 0)
                    {
                        logoUrl = "";
                    }

                    object[] data1 = new object[8];
                    data1[0] = shopName;
                    data1[1] = "";
                    data1[2] = address;
                    data1[3] = categoryNameKor;
                    data1[4] = shopNumber;
                    data1[5] = lat;
                    data1[6] = lng;
                    data1[7] = logoUrl;

                    dataGridView3.Rows.Add(data1);
                }
                npage++;

                if (ntotal <= (npage * nlimit))
                {
                    break;
                }
            }
        }
        #endregion

        #region -배민 매장 상세 조회
        private void GetBaeminShopDetail(string shop_cd, string lat, string lng)
        {
            //실시간 배민데이터 가져오기
            SScraping _sc = new SScraping();
            CookieCollection cookieCollection = new CookieCollection();
            CookieContainer cookieContainer = new CookieContainer();

            //매장상세정보
            string baeminShopcd = shop_cd;
            string strUrl = "https://shopdp-api.baemin.com/v8/shop/" + baeminShopcd + "/detail?lat=" + lat + "&lng=" + lng + "&limit=25&mem=&memid=&defaultreview=N&campaignId=&displayGroup=BAEMIN&lat4Distance=" + lat + "&lng4Distance=" + lng + "&appver=10.9.1&carrier=&site=7jWXRELC2e&deviceModel=SM-G973N&dvcid=OPUD4bf43dcba41eff4a&adid=NONE&sessionId=&osver=22&oscd=2";
            string strReturn = _sc.GetHtmlSource(strUrl, "utf-8", "", "", ref cookieCollection, ref cookieContainer);
            JavaScriptSerializer jss2 = new JavaScriptSerializer();
            dynamic data2 = jss2.Deserialize<dynamic>(strReturn);
            string status = data2["status"] == null ? "" : data2["status"].ToString();
            if (status != "SUCCESS")
            {
                MessageBox.Show("매장을 조회하지 못했습니다.");
                return;
            }
            dynamic shop_info = data2["data"]["shop_info"];

            //매장기본정보 조회
            string Addr = shop_info["Addr"] == null ? "" : shop_info["Addr"].ToString();    //주소
            string Biz_No = shop_info["Biz_No"] == null ? "" : shop_info["Biz_No"].ToString();    //사업자번호
            string Ceo_Nm = shop_info["Ceo_Nm"] == null ? "" : shop_info["Ceo_Nm"].ToString();    //대표자명
            string Tel_No = shop_info["Tel_No"] == null ? "" : shop_info["Tel_No"].ToString();    //전화번호
            string Close_Day = shop_info["Close_Day"] == null ? "" : shop_info["Close_Day"].ToString();    //휴일(매주 일요일, 연중무휴)
            string Ct_Cd = shop_info["Ct_Ty_Cd"] == null ? "" : shop_info["Ct_Ty_Cd"].ToString();    //카테고리코드
            string Ct_Cd_Nm = shop_info["Ct_Cd_Nm"] == null ? "" : shop_info["Ct_Cd_Nm"].ToString();    //카테고리명
            string Dlvry_Tm = shop_info["Dlvry_Tm"] == null ? "" : shop_info["Dlvry_Tm"].ToString();    //배달시간(Dlvry_Tm=평일, 토요일 - 오전 11:00 ~ 오후 8:00)
            string Loc_Pnt_Lng = shop_info["Loc_Pnt_Lng"] == null ? "" : shop_info["Loc_Pnt_Lng"].ToString();    //위도(127.28949163)
            string Loc_Pnt_Lat = shop_info["Loc_Pnt_Lat"] == null ? "" : shop_info["Loc_Pnt_Lat"].ToString();    //경도(36.61379203)
            string Meet_Card = shop_info["Meet_Card"] == null ? "" : shop_info["Meet_Card"].ToString();    //만나서카드결제(Y/N)
            string Meet_Cash = shop_info["Meet_Cash"] == null ? "" : shop_info["Meet_Cash"].ToString();    //만나서현금결제(Y/N)
            string Shop_Intro = shop_info["Shop_Intro"] == null ? "" : shop_info["Shop_Intro"].ToString();    //매장소개
            string Shop_Nm = shop_info["Shop_Nm"] == null ? "" : shop_info["Shop_Nm"].ToString();    //매장명
            string Shop_No = shop_info["Shop_No"] == null ? "" : shop_info["Shop_No"].ToString();    //배민매장코드
            string useDelivery = shop_info["useDelivery"] == null ? "" : shop_info["useDelivery"].ToString();    //배달가능여부(True/False)
            string useTakeout = shop_info["useTakeout"] == null ? "" : shop_info["useTakeout"].ToString();    //픽업가능여부(True/False)
            string takeoutDiscountPrice = shop_info["takeoutDiscountPrice"] == null ? "" : shop_info["takeoutDiscountPrice"].ToString();    //픽업할인금액
            string takeoutDiscountWhenOverMinimumOrderPrice = shop_info["takeoutDiscountWhenOverMinimumOrderPrice"] == null ? "" : shop_info["takeoutDiscountWhenOverMinimumOrderPrice"].ToString();    //최소주문금액 이상일때 픽업할인(True/False)


            object[] data1 = new object[7];
            data1[0] = Shop_Nm;
            data1[1] = Tel_No;
            data1[2] = Addr;
            data1[3] = Ct_Cd_Nm;
            data1[4] = baeminShopcd;
            data1[5] = Loc_Pnt_Lat;
            data1[6] = Loc_Pnt_Lng;

            dataGridView3.Rows.Add(data1);
        }
        #endregion

        #region -배민 매장 조회
        private void GetBaeminShop(string baeminshop_cd, string lat, string lng, bool pImgsave, bool pMImgsave, string logoUrl)
        {
            splash = new SplashThread();
            try
            {

                splash.Open();
                //메뉴그룹
                Dictionary<string, Menu_Group> d_menu_group_list = new Dictionary<string, Menu_Group>();

                //메뉴
                Dictionary<string, Menu> d_menu_list = new Dictionary<string, Menu>();

                //옵션그룹
                Dictionary<string, Option_Group> d_ot_group_list = new Dictionary<string, Option_Group>();

                //실시간 배민데이터 가져오기
                SScraping _sc = new SScraping();
                CookieCollection cookieCollection = new CookieCollection();
                CookieContainer cookieContainer = new CookieContainer();

                //매장상세정보
                string baeminShopcd = baeminshop_cd;
                string strUrl = "https://shopdp-api.baemin.com/v8/shop/" + baeminShopcd + "/detail?lat=" + lat + "&lng=" + lng + "&limit=25&mem=&memid=&defaultreview=N&campaignId=9711657&displayGroup=BAEMIN_DELIVERY_HOME&lat4Distance=" + lat + "&lng4Distance=" + lng + "&filter=&appver=12.23.0&carrier=302780&site=7jWXRELC2e&deviceModel=CPH1823&dvcid=OPUDf48850e556873dfc&adid=NONE&sessionId=&osver=32&oscd=2&ActionTrackingKey=Organic";
                //string strReturn = _sc.GetHtmlSource(finalUrl, "", "", "", ref cookieCollection, ref cookieContainer);
                string strToken = _sc.GetHtmlSource("https://baeminsolver.onrender.com/v1/baemin/solver", "utf-8", "", "", ref cookieCollection, ref cookieContainer);

                var client = new RestClient(strUrl);
                var request = new RestRequest();
                request.AddHeader("Accept-Encoding", "gzip, deflate");
                request.AddHeader("Carrier", "302780");
                request.AddHeader("Connection", "Keep-Alive");
                request.AddHeader("Host", "shopdp-api.baemin.com");
                request.AddHeader("User-Agent", "and1_12.23.0");
                request.AddHeader("USER-BAEDAL", strToken);
                string strReturn = client.ExecuteGet(request).Content;

                JavaScriptSerializer jss2 = new JavaScriptSerializer();
                dynamic data2 = jss2.Deserialize<dynamic>(strReturn);
                string status = data2["status"] == null ? "" : data2["status"].ToString();
                if (status != "SUCCESS")
                {
                    splash.CloseEnd();
                    MessageBox.Show("매장을 조회하지 못했습니다.");
                    return;
                }
                dynamic shop_info = data2["data"]["shop_info"];
                dynamic shop_menu = data2["data"]["shop_menu"];

                //매장기본정보 조회
                //string Addr = shop_info["Addr"] == null ? "" : shop_info["Addr"].ToString().Replace("'", "`");    //주소
                string Addr = shop_info["Business_Location"] == null ? "" : shop_info["Business_Location"].ToString().Replace("'", "`");    //주소
                string Biz_No = shop_info["Biz_No"] == null ? "" : shop_info["Biz_No"].ToString();    //사업자번호
                string Ceo_Nm = shop_info["Ceo_Nm"] == null ? "" : shop_info["Ceo_Nm"].ToString().Replace("'", "`");    //대표자명
                string Tel_No = shop_info["Tel_No"] == null ? "" : shop_info["Tel_No"].ToString();    //전화번호
                string Close_Day = shop_info["Close_Day"] == null ? "" : shop_info["Close_Day"].ToString();    //휴일(매주 일요일, 연중무휴)
                string alwayhol = Close_Day == "연중무휴" ? "1" : "0";
                string Ct_Cd = shop_info["Ct_Ty_Cd"] == null ? "" : shop_info["Ct_Ty_Cd"].ToString();    //카테고리코드
                string Ct_Cd_Nm = shop_info["Ct_Cd_Nm"] == null ? "" : shop_info["Ct_Cd_Nm"].ToString().Replace("'", "`");    //카테고리명
                //카페·디저트
                //카페/디저트
                Ct_Cd_Nm = Ct_Cd_Nm.Replace("·", "/");
                string Dlvry_Tm = shop_info["Dlvry_Tm"] == null ? "" : shop_info["Dlvry_Tm"].ToString();    //배달시간(Dlvry_Tm=평일, 토요일 - 오전 11:00 ~ 오후 8:00)
                string Break_Tm_Info = shop_info["Break_Tm_Info"] == null ? "" : shop_info["Break_Tm_Info"].ToString();    //브레이크타임(오후 2:30 ~ 오후 3:30)
                string Loc_Pnt_Lng = shop_info["Loc_Pnt_Lng"] == null ? "" : shop_info["Loc_Pnt_Lng"].ToString();    //위도(127.28949163)
                string Loc_Pnt_Lat = shop_info["Loc_Pnt_Lat"] == null ? "" : shop_info["Loc_Pnt_Lat"].ToString();    //경도(36.61379203)
                string Meet_Card = shop_info["Meet_Card"] == null ? "" : shop_info["Meet_Card"].ToString();    //만나서카드결제(Y/N)
                Meet_Card = Meet_Card == "Y" ? "1" : "0";
                string Meet_Cash = shop_info["Meet_Cash"] == null ? "" : shop_info["Meet_Cash"].ToString();    //만나서현금결제(Y/N)
                Meet_Cash = Meet_Cash == "Y" ? "1" : "0";
                string Meet_pay = "0";
                if (Meet_Card == "1" || Meet_Cash == "1")
                    Meet_pay = "1";
                string Shop_Intro = shop_info["Shop_Intro"] == null ? "" : shop_info["Shop_Intro"].ToString().Replace("'", "`");    //매장소개
                string Shop_Nm = shop_info["Shop_Nm"] == null ? "" : shop_info["Shop_Nm"].ToString().Replace("'", "`");    //매장명
                string Shop_No = shop_info["Shop_No"] == null ? "" : shop_info["Shop_No"].ToString();    //배민매장코드
                string useDelivery = shop_info["useDelivery"] == null ? "" : shop_info["useDelivery"].ToString();    //배달가능여부(True/False)
                useDelivery = useDelivery == "True" ? "1" : "0";
                string useTakeout = shop_info["useTakeout"] == null ? "" : shop_info["useTakeout"].ToString();    //픽업가능여부(True/False)
                useTakeout = useTakeout == "True" ? "1" : "0";
                string useReservedOrder = shop_info["useReservedOrder"] == null ? "" : shop_info["useReservedOrder"].ToString();    //예약주문여부(True/False)
                useReservedOrder = useReservedOrder == "True" ? "1" : "0";
                string takeoutDiscountPrice = shop_info["takeoutDiscountPrice"] == null ? "" : shop_info["takeoutDiscountPrice"].ToString();    //픽업할인금액
                takeoutDiscountPrice = takeoutDiscountPrice.Replace(",", "").Replace("원", "").Replace(" ", "");
                string takeoutDiscountYn = takeoutDiscountPrice == "0" ? "0" : "1";
                string takeoutDiscountWhenOverMinimumOrderPrice = shop_info["takeoutDiscountWhenOverMinimumOrderPrice"] == null ? "" : shop_info["takeoutDiscountWhenOverMinimumOrderPrice"].ToString();    //최소주문금액 이상일때 픽업할인(True/False)
                takeoutDiscountWhenOverMinimumOrderPrice = takeoutDiscountWhenOverMinimumOrderPrice == "True" ? "1" : "0";
                string Dlvry_Info = shop_info["Dlvry_Info"] == null ? "" : shop_info["Dlvry_Info"].ToString().Replace("'", "`");    //배달정보
                string shop_tip_amt = "0";

                string Att_Cont = shop_menu["menu_info"]["Att_Cont"] == null ? "" : shop_menu["menu_info"]["Att_Cont"].ToString().Replace("'", "`");    //주문안내
                string Food_Org = shop_menu["menu_info"]["Food_Org"] == null ? "" : shop_menu["menu_info"]["Food_Org"].ToString().Replace("'", "`");    //원산지
                string Min_Ord_Price_Txt = shop_menu["menu_info"]["Min_Ord_Price"] == null ? "" : shop_menu["menu_info"]["Min_Ord_Price"].ToString();    //최소주문금액 : 10,000원
                Min_Ord_Price_Txt = Min_Ord_Price_Txt.Replace("최소주문금액", "").Replace(":", "").Replace(",", "").Replace("원", "").Replace(" ", "");
                dynamic menugroup = shop_menu["menu_ord"]["groupMenus"];
                foreach (dynamic menuGroup in shop_menu["menu_ord"]["groupMenus"])
                {
                    Menu_Group d_menu_group = new Menu_Group();
                    //d_menu_group_list
                    string menuGroupId = menuGroup["menuGroupId"] == null ? "" : menuGroup["menuGroupId"].ToString();   //메뉴그룹코드
                    string menuGroupName = menuGroup["name"] == null ? "" : menuGroup["name"].ToString().Replace("'", "`");   //메뉴그룹명
                    string menuGroupAlcohol = "0";

                    int tx = 0; //전체갯수
                    int ax = 0; //알콜갯수
                    foreach (dynamic menus in menuGroup["menus"])
                    {
                        Menu d_menu = new Menu();

                        string menuId = menus["menuId"] == null ? "" : menus["menuId"].ToString();   //메뉴코드
                        string menu_name = menus["name"] == null ? "" : menus["name"].ToString().Replace("'", "`");   //메뉴명
                        string menu_description = menus["description"] == null ? "" : menus["description"].ToString();    //메뉴설명
                        string menu_soldOut = menus["soldOut"] == null ? "" : menus["soldOut"].ToString();    //품절(True/False)
                        string liquor = menus["liquor"] == null ? "" : menus["liquor"].ToString();   //주류여부(True/False)
                        string menu_alcohol = "0";
                        if (liquor == "True")
                        {
                            liquor = "1";
                            menu_alcohol = "1";
                            ax++;
                        }
                        else
                        {
                            liquor = "0";
                            menu_alcohol = "0";
                        }
                        if (menu_soldOut == "True")
                        {
                            menu_soldOut = "1";
                        }
                        else
                        {
                            menu_soldOut = "0";
                        }
                        string mainYn = "";   //대표여부(대표)
                        mainYn = "0";
                        if (menus["badges"] != null)
                        {
                            if (menus["badges"].Length > 0)
                            {
                                foreach (dynamic badges in menus["badges"])
                                {
                                    mainYn = badges["text"] == null ? "" : badges["text"].ToString();
                                    if (mainYn == "대표")
                                    {
                                        mainYn = "1";
                                        break;
                                    }
                                    else
                                    {
                                        mainYn = "0";
                                    }
                                }
                            }
                        }


                        d_menu.menu_group_cd = menuGroupId;
                        d_menu.tmp_menu_cd = menuId;
                        d_menu.menu_name = menu_name;
                        d_menu.menu_main = mainYn;
                        d_menu.menu_solout = menu_soldOut;
                        d_menu.menu_description = menu_description;
                        d_menu.menu_component = "";
                        d_menu.menu_image = "";
                        d_menu.menu_alcohol = menu_alcohol;



                        //메뉴리스트 조회한다.
                        strUrl = "https://shopdp-api.baemin.com/v1/shops/" + baeminShopcd + "/menus/" + menuId + "?appver=10.9.1&carrier=&site=7jWXRELC2e&deviceModel=SM-G973N&dvcid=OPUD4bf43dcba41eff4a&adid=NONE&sessionId=&osver=22&oscd=2";
                        strReturn = _sc.GetHtmlSource(strUrl, "utf-8", "", "", ref cookieCollection, ref cookieContainer);
                        JavaScriptSerializer jss3 = new JavaScriptSerializer();
                        dynamic data3 = jss3.Deserialize<dynamic>(strReturn);
                        string status3 = data3["status"] == null ? "" : data3["status"].ToString();
                        if (status3 != "SUCCESS")
                        {
                            continue;
                        }
                        dynamic menusList = data3["data"];

                        //메뉴이미지
                        string nomalbimg = "";
                        string shumbimg = "";
                        foreach (dynamic menuImages in menusList["images"])
                        {
                            string imgtype = menuImages["type"] == null ? "" : menuImages["type"].ToString().Replace("'", "`");    //메뉴이미지타입
                            string imgurl = menuImages["url"] == null ? "" : menuImages["url"].ToString().Replace("'", "`");    //메뉴이미지url
                            if (imgtype == "NORMAL")
                            {
                                nomalbimg = imgurl;
                            }
                            if (imgtype == "THUMBNAIL")
                            {
                                shumbimg = imgurl;
                            }
                        }
                        if (shumbimg != "")
                        {
                            d_menu.menu_image = shumbimg;
                        }
                        else
                        {
                            d_menu.menu_image = nomalbimg;
                        }

                        if (!pImgsave)
                        {
                            d_menu.menu_image = "";
                        }

                        //메뉴가격
                        foreach (dynamic menuPrice in menusList["menuPrice"]["options"])
                        {
                            Menu_Price price = new Menu_Price();
                            string price_name = menuPrice["name"] == null ? "" : menuPrice["name"].ToString().Replace("'", "`");    //메뉴가격명
                            string price_price = menuPrice["price"] == null ? "" : menuPrice["price"].ToString();    //메뉴가격
                            price_price = price_price.Replace(",", "").Replace("원", "").Replace(" ", "");
                            price.menu_price_name = price_name;
                            price.menu_price_amount = price_price;

                            d_menu.price.Add(price);
                        }

                        //메뉴옵션
                        foreach (dynamic optinGroup in menusList["optionGroups"])
                        {
                            Option_Group d_ot_group = new Option_Group();
                            string optionGId = optinGroup["optionGroupId"] == null ? "" : optinGroup["optionGroupId"].ToString();    //옵션그룹코드
                            string optionGNm = optinGroup["name"] == null ? "" : optinGroup["name"].ToString().Replace("'", "`");    //옵션그룹명
                            string optionGSelMax = optinGroup["maxOrderableQuantity"] == null ? "" : optinGroup["maxOrderableQuantity"].ToString();    //최대선택가능
                            string optionGSelMin = optinGroup["minOrderableQuantity"] == null ? "" : optinGroup["minOrderableQuantity"].ToString();    //최소선택가능
                            string min_order_sel = "0";
                            optionGSelMax = optionGSelMax.Replace(",", "").Replace("개", "").Replace(" ", "");
                            optionGSelMin = optionGSelMin.Replace(",", "").Replace("개", "").Replace(" ", "");
                            if (optionGSelMin == "1" && optionGSelMax == "1")
                            {
                                min_order_sel = "1";
                            }
                            if (optionGId == "73368736")
                            {
                                //잡아라..
                                int xxa = 1;
                                int xxaa = 2;
                                int xxvv = 0;
                                xxvv = xxa * xxaa;
                            }

                            d_ot_group.tmp_option_group_cd = optionGId;
                            d_ot_group.option_group_name = optionGNm;
                            d_ot_group.gubun = "1";
                            //옵션
                            bool bsave = false;
                            string menu_tmp_option_group_cd = "";
                            foreach (dynamic option in optinGroup["options"])
                            {
                                string optionId = option["optionId"] == null ? "" : option["optionId"].ToString();    //옵션코드
                                string optionNm = option["name"] == null ? "" : option["name"].ToString().Replace("'", "`");    //옵션명
                                string optionPrice = option["price"] == null ? "" : option["price"].ToString();    //옵션가격
                                optionPrice = optionPrice.Replace(",", "").Replace("원", "").Replace(" ", "");

                                Option d_ot = new Option();
                                d_ot.tmp_option_group_cd = optionGId;
                                d_ot.tmp_option_cd = optionId;
                                d_ot.option_name = optionNm;
                                d_ot.option_amount = optionPrice;
                                d_ot.option_hide = "0";
                                d_ot.option_soldout = "0";
                                d_ot.option_gubun = "1";

                                d_ot_group.options.Add(d_ot);
                            }

                            //이미들어가 있는건 안넣는다. 코드로 매핑시키면 안됨.(배민은 코드를 새로 생성함.)
                            foreach (KeyValuePair<string, Option_Group> tkvp in d_ot_group_list)
                            {
                                string tmp_option_group_cd = tkvp.Key;
                                Option_Group option_group = tkvp.Value;

                                //옵션그룹명이 같고, 옵션갯수가 같으면 일단 비교해본다.
                                if (option_group.option_group_name == d_ot_group.option_group_name && d_ot_group.options.Count == option_group.options.Count)
                                {
                                    int ncomnp = 0;
                                    for (int ssi = 0; ssi < d_ot_group.options.Count; ssi++)
                                    {
                                        //옵션금액,옵션명,배민옵션코드가 같으면 같은 옵션그룹으로 처리한다.
                                        if (d_ot_group.options[ssi].option_amount == option_group.options[ssi].option_amount &&
                                            d_ot_group.options[ssi].option_name == option_group.options[ssi].option_name &&
                                            d_ot_group.options[ssi].tmp_option_cd == option_group.options[ssi].tmp_option_cd)
                                        {
                                            ncomnp++;
                                        }
                                    }
                                    if (ncomnp == d_ot_group.options.Count)
                                    {
                                        //같은 옵션이 들어가 있으면 같이 사용한다.
                                        string toption_group_cd = option_group.tmp_option_group_cd;  //메뉴옵션그룹명
                                        bsave = true;
                                        menu_tmp_option_group_cd = toption_group_cd;
                                        break;
                                    }
                                }
                                /*
                                List<Option> topt = option_group.options;
                                for (int k = 0; k < topt.Count; k++)
                                {
                                    Option top = topt[k];
                                    string toption_group_cd = option_group.tmp_option_group_cd;  //메뉴옵션그룹명
                                    string toption_group_name = option_group.option_group_name;  //메뉴옵션그룹명
                                    string toption_id = top.tmp_option_cd;  //메뉴옵션코드
                                    string toption_name = top.option_name;  //메뉴옵션명
                                    string toption_amount = top.option_amount;   //메뉴옵션금액

                                    if (toption_group_name == optionGNm && toption_name == optionNm && toption_amount == optionPrice && toption_id == optionId)
                                    {
                                        bsave = true;
                                        menu_tmp_option_group_cd = toption_group_cd;
                                        break;
                                    }
                                }
                                */
                            }

                            if (!bsave)
                            {
                                //옵션그룹넣자!!
                                if (!d_ot_group_list.ContainsKey(optionGId))
                                {
                                    d_ot_group_list.Add(optionGId, d_ot_group);
                                }
                            }

                            //메뉴-옵션그룹
                            Menu_Option_Group mog = new Menu_Option_Group();
                            if (bsave)
                            {
                                mog.tmp_option_group_cd = menu_tmp_option_group_cd;
                            }
                            else
                            {
                                mog.tmp_option_group_cd = optionGId;
                            }
                            mog.max_order_qty = optionGSelMax;
                            mog.min_order_qty = optionGSelMin;
                            mog.min_order_sel = min_order_sel;

                            d_menu.options.Add(mog);
                        }
                        //메뉴넣자!!
                        if (!d_menu_list.ContainsKey(menuId))
                        {
                            d_menu_list.Add(menuId, d_menu);
                            d_menu_group.menu.Add(d_menu);
                        }
                        tx++;
                    }

                    if ((ax == tx && ax > 0) || menuGroupName == "주류메뉴")
                    {
                        //주류그룹이다.
                        menuGroupAlcohol = "1";
                    }
                    //배민의 대표메뉴 라는 그룹명은 배슐랭에서는 사용못한다.
                    //그래서 그냥 대표메뉴 --> 대표 메뉴로 저장한다.
                    if (menuGroupName == "대표메뉴")
                    {
                        menuGroupName = "대표 메뉴";
                    }
                    d_menu_group.menu_group_cd = "";
                    d_menu_group.menu_group_name = menuGroupName;
                    d_menu_group.menu_group_hide = "0";
                    d_menu_group.menu_group_soldout = "0";
                    d_menu_group.menu_group_alcohol = menuGroupAlcohol;
                    d_menu_group.gubun = "1";

                    //메뉴그룹넣자!!
                    if (!d_menu_group_list.ContainsKey(menuGroupId))
                    {
                        d_menu_group_list.Add(menuGroupId, d_menu_group);
                    }

                }

                //세트메뉴그룹
                bool bCheckMenu = false;
                try
                {
                    if(shop_menu["menu_ord"]["setMenus"] != null)
                    {
                        bCheckMenu = true;
                    }
                }
                catch(Exception ex)
                {

                }
                if (bCheckMenu)
                {
                    if (shop_menu["menu_ord"]["setMenus"].Length > 0)
                    {
                        Menu_Group d_menu_group = new Menu_Group();
                        //d_menu_group_list
                        string menuGroupId = "0";   //메뉴그룹코드
                        string menuGroupName = "세트메뉴";   //메뉴그룹명
                        string menuGroupAlcohol = "0";

                        foreach (dynamic menus in shop_menu["menu_ord"]["setMenus"])
                        {
                            Menu d_menu = new Menu();

                            string menuId = menus["menuId"] == null ? "" : menus["menuId"].ToString();   //메뉴코드
                            string menu_name = menus["name"] == null ? "" : menus["name"].ToString().Replace("'", "`");   //메뉴명
                            string menu_description = menus["description"] == null ? "" : menus["description"].ToString().Replace("'", "`");    //메뉴설명
                            string menu_soldOut = menus["soldOut"] == null ? "" : menus["soldOut"].ToString();    //품절(True/False)
                            string liquor = menus["liquor"] == null ? "" : menus["liquor"].ToString();   //주류여부(True/False)
                            string menu_alcohol = "0";
                            if (liquor == "True")
                            {
                                liquor = "1";
                                menu_alcohol = "1";
                            }
                            else
                            {
                                liquor = "0";
                                menu_alcohol = "0";
                            }
                            if (menu_soldOut == "True")
                            {
                                menu_soldOut = "1";
                            }
                            else
                            {
                                menu_soldOut = "0";
                            }
                            string mainYn = "";   //대표여부(대표)
                            mainYn = "0";
                            if (menus["badges"] != null)
                            {
                                if (menus["badges"].Length > 0)
                                {
                                    foreach (dynamic badges in menus["badges"])
                                    {
                                        mainYn = badges["text"] == null ? "" : badges["text"].ToString();
                                        if (mainYn == "대표")
                                        {
                                            mainYn = "1";
                                            break;
                                        }
                                        else
                                        {
                                            mainYn = "0";
                                        }
                                    }
                                }
                            }

                            d_menu.menu_group_cd = menuGroupId;
                            d_menu.tmp_menu_cd = menuId;
                            d_menu.menu_name = menu_name;
                            d_menu.menu_main = mainYn;
                            d_menu.menu_solout = menu_soldOut;
                            d_menu.menu_description = menu_description;
                            d_menu.menu_component = "";
                            d_menu.menu_image = "";
                            d_menu.menu_alcohol = menu_alcohol;



                            //메뉴리스트 조회한다.
                            strUrl = "https://shopdp-api.baemin.com/v1/shops/" + baeminShopcd + "/menus/" + menuId + "?appver=10.9.1&carrier=&site=7jWXRELC2e&deviceModel=SM-G973N&dvcid=OPUD4bf43dcba41eff4a&adid=NONE&sessionId=&osver=22&oscd=2";
                            strReturn = _sc.GetHtmlSource(strUrl, "utf-8", "", "", ref cookieCollection, ref cookieContainer);
                            JavaScriptSerializer jss3 = new JavaScriptSerializer();
                            dynamic data3 = jss3.Deserialize<dynamic>(strReturn);
                            string status3 = data3["status"] == null ? "" : data3["status"].ToString();
                            if (status3 != "SUCCESS")
                            {
                                continue;
                            }
                            dynamic menusList = data3["data"];

                            //메뉴이미지
                            string nomalbimg = "";
                            string shumbimg = "";
                            foreach (dynamic menuImages in menusList["images"])
                            {
                                string imgtype = menuImages["type"] == null ? "" : menuImages["type"].ToString().Replace("'", "`");    //메뉴이미지타입
                                string imgurl = menuImages["url"] == null ? "" : menuImages["url"].ToString().Replace("'", "`");    //메뉴이미지url
                                if (imgtype == "NORMAL")
                                {
                                    nomalbimg = imgurl;
                                }
                                if (imgtype == "THUMBNAIL")
                                {
                                    shumbimg = imgurl;
                                }
                            }
                            if (shumbimg != "")
                            {
                                d_menu.menu_image = shumbimg;
                            }
                            else
                            {
                                d_menu.menu_image = nomalbimg;
                            }

                            if (!pImgsave)
                            {
                                d_menu.menu_image = "";
                            }

                            //메뉴가격
                            foreach (dynamic menuPrice in menusList["menuPrice"]["options"])
                            {
                                Menu_Price price = new Menu_Price();
                                string price_name = menuPrice["name"] == null ? "" : menuPrice["name"].ToString().Replace("'", "`");    //메뉴가격명
                                string price_price = menuPrice["price"] == null ? "" : menuPrice["price"].ToString();    //메뉴가격
                                price_price = price_price.Replace(",", "").Replace("원", "").Replace(" ", "");
                                price.menu_price_name = price_name;
                                price.menu_price_amount = price_price;

                                d_menu.price.Add(price);
                            }

                            //메뉴옵션
                            foreach (dynamic optinGroup in menusList["optionGroups"])
                            {
                                Option_Group d_ot_group = new Option_Group();
                                string optionGId = optinGroup["optionGroupId"] == null ? "" : optinGroup["optionGroupId"].ToString();    //옵션그룹코드
                                string optionGNm = optinGroup["name"] == null ? "" : optinGroup["name"].ToString().Replace("'", "`");    //옵션그룹명
                                string optionGSelMax = optinGroup["maxOrderableQuantity"] == null ? "" : optinGroup["maxOrderableQuantity"].ToString();    //최대선택가능
                                string optionGSelMin = optinGroup["minOrderableQuantity"] == null ? "" : optinGroup["minOrderableQuantity"].ToString();    //최소선택가능
                                string min_order_sel = "0";
                                optionGSelMax = optionGSelMax.Replace(",", "").Replace("개", "").Replace(" ", "");
                                optionGSelMin = optionGSelMin.Replace(",", "").Replace("개", "").Replace(" ", "");
                                if (optionGSelMin == "1" && optionGSelMax == "1")
                                {
                                    min_order_sel = "1";
                                }

                                d_ot_group.tmp_option_group_cd = optionGId;
                                d_ot_group.option_group_name = optionGNm;
                                d_ot_group.gubun = "1";
                                //옵션
                                foreach (dynamic option in optinGroup["options"])
                                {
                                    string optionId = option["optionId"] == null ? "" : option["optionId"].ToString();    //옵션코드
                                    string optionNm = option["name"] == null ? "" : option["name"].ToString().Replace("'", "`");    //옵션명
                                    string optionPrice = option["price"] == null ? "" : option["price"].ToString();    //옵션가격
                                    optionPrice = optionPrice.Replace(",", "").Replace("원", "").Replace(" ", "");

                                    Option d_ot = new Option();
                                    d_ot.tmp_option_group_cd = optionGId;
                                    d_ot.tmp_option_cd = optionId;
                                    d_ot.option_name = optionNm;
                                    d_ot.option_amount = optionPrice;
                                    d_ot.option_hide = "0";
                                    d_ot.option_soldout = "0";
                                    d_ot.option_gubun = "1";

                                    d_ot_group.options.Add(d_ot);
                                }

                                //옵션그룹넣자!!
                                if (!d_ot_group_list.ContainsKey(optionGId))
                                {
                                    d_ot_group_list.Add(optionGId, d_ot_group);
                                }

                                //메뉴-옵션그룹
                                Menu_Option_Group mog = new Menu_Option_Group();
                                mog.tmp_option_group_cd = optionGId;
                                mog.max_order_qty = optionGSelMax;
                                mog.min_order_qty = optionGSelMin;
                                mog.min_order_sel = min_order_sel;

                                d_menu.options.Add(mog);
                            }
                            //메뉴넣자!!
                            if (!d_menu_list.ContainsKey(menuId))
                            {
                                d_menu_list.Add(menuId, d_menu);
                                d_menu_group.menu.Add(d_menu);
                            }
                        }
                        d_menu_group.menu_group_cd = "";
                        d_menu_group.menu_group_name = menuGroupName;
                        d_menu_group.menu_group_hide = "0";
                        d_menu_group.menu_group_soldout = "0";
                        d_menu_group.menu_group_alcohol = menuGroupAlcohol;
                        d_menu_group.gubun = "1";

                        //메뉴그룹넣자!!
                        if (!d_menu_group_list.ContainsKey(menuGroupId))
                        {
                            d_menu_group_list.Add(menuGroupId, d_menu_group);
                        }
                    }
                }

                string strqry = "";

                if (dataGridView2.CurrentRow == null)
                {
                    splash.CloseEnd();
                    MessageBox.Show("입점신청 매장을 선택해주세요.");
                    return;
                }

                string req_cd = dataGridView2.SelectedRows[0].Cells["req_cd"].Value.ToString();
                string biz_cd = dataGridView2.SelectedRows[0].Cells["biz_cd"].Value.ToString();
                string req_bizno = dataGridView2.SelectedRows[0].Cells["req_bizno"].Value.ToString();
                string req_zipcd = dataGridView2.SelectedRows[0].Cells["req_zipcd"].Value.ToString();
                string biz_acc_cd = dataGridView2.SelectedRows[0].Cells["biz_acc_cd"].Value.ToString();
                string new_type = dataGridView2.SelectedRows[0].Cells["new_type"].Value.ToString();
                string req_bankcd = dataGridView2.SelectedRows[0].Cells["req_bankcd"].Value.ToString();
                string req_acctno = dataGridView2.SelectedRows[0].Cells["req_acctno"].Value.ToString();
                string req_acctowner = dataGridView2.SelectedRows[0].Cells["req_acctowner"].Value.ToString();
                string req_acctimg = dataGridView2.SelectedRows[0].Cells["req_acctimg"].Value.ToString();
                string req_shop_cd = dataGridView2.SelectedRows[0].Cells["req_shop_cd"].Value.ToString();
                //매장정보를 가져온다.
                string shop_cd = GetBaeminShopData(baeminShopcd, req_bizno);

                if (!pMImgsave)
                {
                    logoUrl = "";
                }
                //여기서 매장정보를 저장한다.
                if (shop_cd == "")
                {
                    //logoUrl
                    if (logoUrl != "")
                    {
                        try
                        {
                            //aws
                            DateTime span = DateTime.Now;
                            string fname = span.ToString(@"yyyyMMddHHmmssfff") + ".jpg";
                            SaveImageAws(shop_cd, logoUrl, fname);
                            logoUrl = "https://s3.baechelin.com/media/shop/" + shop_cd + "/" + fname;
                        }
                        catch (Exception)
                        {
                            logoUrl = "";
                        }
                    }
                    if (req_shop_cd == "")
                    {
                        //strqry = "INSERT INTO TB_SHOP_LIST(biz_cd, shop_name, shop_bizno, shop_taxgubun, shop_bizgubun, shop_zipcd, shop_addr_doro, shop_lat, shop_lng, shop_status, new_type, shop_regdt, shop_joinyn, shop_mngstat, ct_cd, ct_name, tmp_shop_cd) ";
                        //strqry += "VALUES ('" + biz_cd + "', '" + Biz_No.Replace("-", "") + "', '1', '0', '" + Shop_Nm + "', '" + req_zipcd + "', '" + Addr + "', '" + Loc_Pnt_Lat + "', '" + Loc_Pnt_Lng + "', '1', '" + new_type + "', '" + new_type + "', NOW(), 1, 0, '" + Ct_Cd + "', '" + Ct_Cd_Nm + "', '" + baeminShopcd + "');";
                        strqry = "INSERT INTO TB_SHOP_LIST (biz_cd, shop_name, shop_bizno, shop_bizimg, shop_regdocimg, shop_taxgubun, shop_bizgubun, shop_zipcd,	shop_addr_doro, shop_addr_jibun, shop_addr_detail, shop_lat, shop_lng, shop_status, shop_img, shop_regdt, shop_joinyn, new_type, shop_mngstat, ct_cd, ct_name, tmp_shop_cd) ";
                        strqry += "SELECT b.biz_cd, a.req_nm, a.req_bizno, a.req_bizimg, a.req_regdocimg, '1', '0', 	a.req_zipcd, a.req_add_doro, a.req_add_jibun, a.req_addr_detail, '" + Loc_Pnt_Lat + "', '" + Loc_Pnt_Lng + "', '1', '" + logoUrl + "', a.req_regdtime, (CASE WHEN a.req_gubun = 3 THEN 1 ELSE 0 END), a.new_type, 0, '" + Ct_Cd + "', '" + Ct_Cd_Nm + "', '" + baeminShopcd + "' ";
                        strqry += "FROM TB_SHOP_REGIST_REQ a INNER JOIN TB_BIZ_LIST b ON a.biz_cd = b.biz_cd ";
                        strqry += "WHERE a.req_cd = " + req_cd + ";";
                        shop_cd = SaveDBData(strqry);
                    }
                    else
                    {
                        shop_cd = req_shop_cd;
                        strqry = "UPDATE TB_SHOP_LIST SET shop_img = '" + logoUrl + "', shop_lat = '" + Loc_Pnt_Lat + "', ct_name = '" + Ct_Cd_Nm + "', shop_lng = '" + Loc_Pnt_Lng + "', shop_regdt = NOW(), shop_joinyn = 1 WHERE shop_cd = " + shop_cd + ";";
                        SaveDBData(strqry);
                    }
                }
                else
                {
                    //logoUrl
                    if (logoUrl != "")
                    {
                        try
                        {
                            //aws
                            DateTime span = DateTime.Now;
                            string fname = span.ToString(@"yyyyMMddHHmmssfff") + ".jpg";
                            SaveImageAws(shop_cd, logoUrl, fname);
                            logoUrl = "https://s3.baechelin.com/media/shop/" + shop_cd + "/" + fname;
                        }
                        catch (Exception)
                        {
                            logoUrl = "";
                        }
                    }

                    if (shop_cd != req_shop_cd && req_shop_cd != "")
                    {
                        splash.CloseEnd();
                        MessageBox.Show("이미 매장이 연결된 매장입니다.\r\n관리자에게 문의하세요!");

                        button8.Visible = true;
                        return;
                    }
                    strqry = "UPDATE TB_SHOP_LIST SET shop_img = '" + logoUrl + "', shop_addr_doro = '" + Addr + "', shop_lat = '" + Loc_Pnt_Lat + "', shop_lng = '" + Loc_Pnt_Lng + "', ct_name = '" + Ct_Cd_Nm + "', shop_regdt = NOW(), shop_joinyn = 1 WHERE tmp_shop_cd = " + baeminShopcd + ";";
                    SaveDBData(strqry);
                }

                //매장이 속해있는 동은 기본으로 배달지역에 넣는다.
                string searAddr = "";
                if (dataGridView2.CurrentRow == null)
                {
                    searAddr = Addr;
                }
                else
                {
                    searAddr = dataGridView2.SelectedRows[0].Cells["req_add_doro"].Value.ToString();

                    if (searAddr == "")
                    {
                        searAddr = dataGridView3.SelectedRows[0].Cells["bm_address"].Value.ToString().Trim();
                    }
                }

                strqry = "DELETE FROM TB_SHOP_AREA WHERE shop_cd = " + shop_cd + ";";
                SaveDBData(strqry);
                //매장상세정보
                string strUrl_addr = "https://mapi.baechelin.com/mobile/api/v1/addresses?keyword=" + searAddr;
                string strReturn_addr = _sc.GetHtmlSource(strUrl_addr, "utf-8", "", "", ref cookieCollection, ref cookieContainer);
                JavaScriptSerializer jss2_addr = new JavaScriptSerializer();
                dynamic data2_addr = jss2_addr.Deserialize<dynamic>(strReturn_addr);

                string siNm = "";
                string emdNm = "";
                string zipCode = "";

                foreach (dynamic data3 in data2_addr)
                {
                    siNm = data3["siNm"] == null ? "" : data3["siNm"].ToString();
                    emdNm = data3["emdNm"] == null ? "" : data3["emdNm"].ToString();
                    zipCode = data3["zipNo"] == null ? "" : data3["zipNo"].ToString();
                    break;
                }
                strqry = "UPDATE TB_SHOP_LIST SET shop_zipcd = '" + zipCode + "' WHERE shop_cd = " + shop_cd + ";";
                SaveDBData(strqry);

                strqry = "UPDATE TB_SHOP_REGIST_REQ SET req_zipcd = '" + zipCode + "' WHERE req_cd = " + req_cd + ";";
                SaveDBData(strqry);

                strqry = "UPDATE TB_BIZ_LIST SET biz_zipcd = '" + zipCode + "' WHERE biz_cd = " + biz_cd + ";";
                SaveDBData(strqry);

                strqry = "INSERT INTO TB_SHOP_AREA (shop_cd, area_gu_cd, area_si, area_gu, area_dong, area_regdt) ";
                strqry += "SELECT '" + shop_cd + "', sgg_code, sido_name, sgg_name, dong_name, NOW() FROM TB_ADDRESS_DONG WHERE sido_name = '" + siNm + "' AND dong_name = '" + emdNm + "';";
                SaveDBData(strqry);

                //배달팁
                strqry = "DELETE FROM TB_SHOP_AMOUNT_TIP WHERE shop_cd = " + shop_cd + ";";
                SaveDBData(strqry);
                strqry = "DELETE FROM TB_SHOP_AREA_TIP WHERE shop_cd = " + shop_cd + ";";
                SaveDBData(strqry);
                strqry = "DELETE FROM TB_SHOP_TIME_TIP WHERE shop_cd = " + shop_cd + ";";
                SaveDBData(strqry);
                if (shop_info["deliveryTip"] != null)
                {
                    if (shop_info["deliveryTip"]["deliveryTipInformation"] != null)
                    {
                        foreach (dynamic deliveryTip in shop_info["deliveryTip"]["deliveryTipInformation"])
                        {
                            string title = deliveryTip["title"] == null ? "" : deliveryTip["title"].ToString();
                            if (title == "주문금액 별 배달팁" /*"기본 배달팁"*/)
                            {
                                //금액별 배달팁
                                foreach (dynamic amt_deliveryTip in deliveryTip["contents"])
                                {
                                    string name = amt_deliveryTip["name"] == null ? "" : amt_deliveryTip["name"].ToString();
                                    string content = amt_deliveryTip["content"] == null ? "" : amt_deliveryTip["content"].ToString();
                                    name = RemoveHtmlCode(name, true);
                                    content = RemoveHtmlCode(content, true);

                                    name = name.Replace("원", "").Replace(",", "").Trim();
                                    content = content.Replace("원", "").Replace(",", "").Replace("이상", "").Replace("이하", "").Trim();
                                    string[] names = name.Split('~');
                                    shop_tip_amt = content;
                                    name = names[0];
                                    name = name.Replace("원", "").Replace(",", "").Replace("이상", "").Replace("이하", "").Trim();

                                    if (Min_Ord_Price_Txt != name)
                                    {
                                        strqry = "INSERT INTO TB_SHOP_AMOUNT_TIP(shop_cd, amt_order_amount, amt_tip_amount, amt_tip_regdt) VALUES (" + shop_cd + ", '" + name + "', '" + content + "', NOW());";
                                        SaveDBData(strqry);
                                    }
                                }
                            }
                            else if (title == "지역별 추가 배달팁")
                            {
                                //지역별 배달팁
                                foreach (dynamic area_deliveryTip in deliveryTip["contents"])
                                {
                                    string name = area_deliveryTip["name"] == null ? "" : area_deliveryTip["name"].ToString();
                                    string content = area_deliveryTip["content"] == null ? "" : area_deliveryTip["content"].ToString();
                                    name = RemoveHtmlCode(name, true);
                                    content = RemoveHtmlCode(content, true);

                                    content = content.Replace("원", "").Replace(",", "").Replace("+", "").Trim();
                                    string[] names = name.Split(',');
                                    string[] addrsidos = Addr.Split(' ');
                                    string addrsido = addrsidos[0];

                                    for (int z = 0; z < names.Length; z++)
                                    {
                                        string dong = names[z];
                                        dong = dong.Replace("원", "").Replace(",", "").Replace("+", "").Trim();
                                        dong = dong.Trim();
                                        strqry = "INSERT INTO TB_SHOP_AREA_TIP (shop_cd, area_tip_amount, area_tip_gu_cd, area_tip_si, area_tip_gu, area_tip_dong, area_tip_regdt) ";
                                        strqry += "SELECT '" + shop_cd + "', '" + content + "', sgg_code, sido_name, sgg_name, dong_name, NOW() FROM TB_ADDRESS_DONG WHERE sido_name = '" + addrsido + "' AND dong_name = '" + dong + "';";
                                        SaveDBData(strqry);

                                        if (siNm != addrsido || emdNm != dong)
                                        {
                                            //일단 배달팁을 배달지역으로 넣는다.
                                            strqry = "INSERT INTO TB_SHOP_AREA (shop_cd, area_gu_cd, area_si, area_gu, area_dong, area_regdt) ";
                                            strqry += "SELECT '" + shop_cd + "', sgg_code, sido_name, sgg_name, dong_name, NOW() FROM TB_ADDRESS_DONG WHERE sido_name = '" + addrsido + "' AND dong_name = '" + dong + "';";
                                            SaveDBData(strqry);
                                        }
                                    }
                                }
                            }
                            else if (title == "그 외 추가 배달팁(중복 적용 가능)")
                            {
                                //시간별 배달팁
                                foreach (dynamic time_deliveryTip in deliveryTip["contents"])
                                {
                                    string mon_open = "0";
                                    string tue_open = "0";
                                    string wed_open = "0";
                                    string thu_open = "0";
                                    string fri_open = "0";
                                    string sat_open = "0";
                                    string sun_open = "0";
                                    int nstime = 0;
                                    int netime = 0;
                                    int nshour = 0;
                                    int nsminute = 0;
                                    int nehour = 0;
                                    int neminute = 0;

                                    string name = time_deliveryTip["name"] == null ? "" : time_deliveryTip["name"].ToString();
                                    string content = time_deliveryTip["content"] == null ? "" : time_deliveryTip["content"].ToString();
                                    name = RemoveHtmlCode(name, true);
                                    content = RemoveHtmlCode(content, true);

                                    content = content.Replace("원", "").Replace(",", "").Replace("+", "").Trim();
                                    if (name.IndexOf("월요일") > -1)
                                    {
                                        mon_open = "1";
                                    }
                                    if (name.IndexOf("화요일") > -1)
                                    {
                                        tue_open = "1";
                                    }
                                    if (name.IndexOf("수요일") > -1)
                                    {
                                        wed_open = "1";
                                    }
                                    if (name.IndexOf("목요일") > -1)
                                    {
                                        thu_open = "1";
                                    }
                                    if (name.IndexOf("금요일") > -1)
                                    {
                                        fri_open = "1";
                                    }
                                    if (name.IndexOf("토요일") > -1)
                                    {
                                        sat_open = "1";
                                    }
                                    if (name.IndexOf("일요일") > -1 || name.IndexOf("공휴일") > -1)
                                    {
                                        sun_open = "1";
                                    }

                                    if (name.IndexOf("~") > -1)
                                    {
                                        string[] names = name.Split('~');
                                        string stime = names[0];
                                        string etime = names[1];
                                        stime = stime.Replace("월요일", "").Replace("화요일", "").Replace("수요일", "").Replace("목요일", "").Replace("금요일", "").Replace("토요일", "").Replace("일요일", "").Replace(",", "").Replace(" ", "");
                                        stime = stime.Trim();
                                        etime = etime.Trim();
                                        string[] stimes = stime.Split(':');
                                        string[] etimes = etime.Split(':');
                                        if (stimes.Length == 2 && etimes.Length == 2)
                                        {
                                            int.TryParse(stimes[0], out nshour);
                                            int.TryParse(stimes[1], out nsminute);
                                            int.TryParse(etimes[0], out nehour);
                                            int.TryParse(etimes[1], out neminute);

                                            nstime = nshour * 60 + nsminute;
                                            netime = nehour * 60 + neminute;
                                        }
                                    }
                                    else
                                    {
                                        nstime = 0;
                                        netime = 0;
                                    }


                                    if (sun_open == "0" && tue_open == "0" && wed_open == "0" && thu_open == "0" && fri_open == "0" && sat_open == "0" && sun_open == "0")
                                    {
                                        continue;
                                    }
                                    strqry = "INSERT INTO TB_SHOP_TIME_TIP(shop_cd,time_tip_begin,time_tip_end,time_tip_mon,time_tip_tue,time_tip_wed,time_tip_thu,time_tip_fri,time_tip_sat,time_tip_sun,time_tip_amount,time_tip_regdt) ";
                                    strqry += "VALUES (" + shop_cd + "," + nstime.ToString() + "," + netime.ToString() + "," + mon_open + "," + tue_open + "," + wed_open + "," + thu_open + "," + fri_open + "," + sat_open + "," + sun_open + "," + content + ", NOW());";
                                    SaveDBData(strqry);
                                }
                            }
                        }
                    }
                }

                //매장운영정보 입력
                /*
                strqry = "UPDATE TB_SHOP_OPERINFO SET shop_able_delivery = '" + useDelivery + "', shop_able_pickup = '" + useTakeout + "', shop_min_amt = '" + Min_Ord_Price_Txt + "', ";
                strqry += "shop_tip_amt = '" + shop_tip_amt + "', shop_pickup_dis = '" + takeoutDiscountYn + "', shop_pickup_dis_amt = '" + takeoutDiscountPrice + "', shop_pickup_min = '" + takeoutDiscountWhenOverMinimumOrderPrice + "', ";
                strqry += "shop_reserv = '" + useReservedOrder + "', shop_intro = '" + Shop_Intro + "', shop_alwaysopen = '" + alwayhol + "', shop_alwaysmemo = '" + Close_Day + "', shop_holinfo = '" + Close_Day + "', ";
                strqry += "shop_holyn = '0', shop_holmsg = '', shop_pay_now = '1', shop_pay_later = '" + Meet_pay + "', shop_pay_card = '" + Meet_Card + "', shop_pay_cash = '" + Meet_Cash + "', shop_is_open = '1', shop_delivery_info = '" + Dlvry_Info + "' WHERE shop_cd = " + shop_cd + ";";
                */
                strqry = "INSERT INTO TB_SHOP_OPERINFO (shop_cd, shop_able_delivery, shop_able_pickup, shop_min_amt, shop_tip_amt, shop_pickup_dis, shop_pickup_dis_amt, shop_pickup_min, shop_reserv, shop_intro, shop_alwaysopen, shop_alwaysmemo, shop_holinfo, shop_holyn, shop_holmsg, shop_pay_now, shop_pay_later, shop_pay_card, shop_pay_cash, shop_is_open, shop_delivery_info) ";
                strqry += "VALUES (" + shop_cd + ",'" + useDelivery + "','" + useTakeout + "','" + Min_Ord_Price_Txt + "','" + shop_tip_amt + "','" + takeoutDiscountYn + "','" + takeoutDiscountPrice + "','" + takeoutDiscountWhenOverMinimumOrderPrice + "','" + useReservedOrder + "','" + Shop_Intro + "','" + alwayhol + "','" + Close_Day + "','" + Close_Day + "','0','','1','" + Meet_pay + "','" + Meet_Card + "','" + Meet_Cash + "','1','" + Dlvry_Info + "') ";
                strqry += "ON DUPLICATE KEY UPDATE ";
                strqry += "shop_able_delivery = '" + useDelivery + "', shop_able_pickup = '" + useTakeout + "', shop_min_amt = '" + Min_Ord_Price_Txt + "', ";
                strqry += "shop_tip_amt = '" + shop_tip_amt + "', shop_pickup_dis = '" + takeoutDiscountYn + "', shop_pickup_dis_amt = '" + takeoutDiscountPrice + "', shop_pickup_min = '" + takeoutDiscountWhenOverMinimumOrderPrice + "', ";
                strqry += "shop_reserv = '" + useReservedOrder + "', shop_intro = '" + Shop_Intro + "', shop_alwaysopen = '" + alwayhol + "', shop_alwaysmemo = '" + Close_Day + "', shop_holinfo = '" + Close_Day + "', ";
                strqry += "shop_holyn = '0', shop_holmsg = '', shop_pay_now = '1', shop_pay_later = '" + Meet_pay + "', shop_pay_card = '" + Meet_Card + "', shop_pay_cash = '" + Meet_Cash + "', shop_is_open = '1', shop_delivery_info = '" + Dlvry_Info + "';";
                SaveDBData(strqry);

                //매장계좌정보 입력
                /*
                strqry = "INSERT INTO TB_SHOP_ACCOUNTINFO (shop_cd, shop_bankcd, shop_acctno, shop_acctowner, shop_acctimg) ";
                strqry += "VALUES (" + shop_cd + ",'" + req_bankcd + "','" + req_acctno + "','" + req_acctowner + "','" + req_acctimg + "') ";
                strqry += "ON DUPLICATE KEY UPDATE ";
                strqry += "shop_bankcd = '" + req_bankcd + "', shop_acctno = '" + req_acctno + "', shop_acctowner = '" + req_acctowner + "', shop_acctimg = '" + req_acctimg + "';";
                SaveDBData(strqry);
                */
                //매장환경설정 입력
                strqry = "INSERT INTO TB_SHOP_CONFIG(shop_cd, section_type) VALUES (" + shop_cd + ", 2) ON DUPLICATE KEY UPDATE section_type=2;";
                SaveDBData(strqry);

                //매장원산지정보 입력
                strqry = "INSERT INTO TB_SHOP_ORIGIN(shop_cd, shop_orderinfo, shop_origin) VALUES (" + shop_cd + ",'" + Att_Cont + "','" + Food_Org + "') ON DUPLICATE KEY UPDATE shop_orderinfo='" + Att_Cont + "', shop_origin='" + Food_Org + "';";
                SaveDBData(strqry);

                //매장회원등급설정 입력
                strqry = "INSERT INTO TB_SHOP_USERLEVEL(shop_cd, section_type) VALUES (" + shop_cd + ", 2) ON DUPLICATE KEY UPDATE section_type=2;";
                SaveDBData(strqry);


                //매장카테고리 입력

                //매장전화번호리스트 입력

                //운영매니저 저장

                //주문접수매니저 저장

                //매장영업시간 입력(Dlvry_Tm)
                strqry = "DELETE FROM TB_SHOP_OPERTIME WHERE shop_cd = " + shop_cd + ";";
                SaveDBData(strqry);

                if (Dlvry_Tm != "")
                {
                    string[] dlvlys = Dlvry_Tm.Split('\n');
                    for (int i = 0; i < dlvlys.Length; i++)
                    {
                        string[] lines = dlvlys[i].Split('-');
                        if (lines.Length == 2)
                        {
                            string mon_open = "0";
                            string tue_open = "0";
                            string wed_open = "0";
                            string thu_open = "0";
                            string fri_open = "0";
                            string sat_open = "0";
                            string sun_open = "0";
                            int nstime = 0;
                            int netime = 0;
                            int nshour = 0;
                            int nsminute = 0;
                            int nehour = 0;
                            int neminute = 0;
                            string stime = "";
                            string etime = "";
                            string weekdays = lines[0];
                            weekdays = weekdays.Trim();
                            string time = lines[1];
                            string[] times = time.Split('~');
                            if (times.Length == 2)
                            {
                                stime = times[0];
                                etime = times[1];
                                stime = stime.Replace("익일 밤", "오전").Replace("익일 새벽", "오전").Replace("익일 오전", "오전").Replace("새벽", "오전").Replace("밤", "오전").Replace("낮", "오후");
                                stime = stime.Trim();
                                etime = etime.Replace("익일 밤", "오전").Replace("익일 새벽", "오전").Replace("익일 오전", "오전").Replace("새벽", "오전").Replace("밤", "오전").Replace("낮", "오후");
                                etime = etime.Trim();

                                stime = DateTime.ParseExact(stime, "tt h:mm", null, System.Globalization.DateTimeStyles.AssumeLocal).ToString("HH:mm");
                                etime = DateTime.ParseExact(etime, "tt h:mm", null, System.Globalization.DateTimeStyles.AssumeLocal).ToString("HH:mm");

                                string[] stimes = stime.Split(':');
                                string[] etimes = etime.Split(':');
                                if (stimes.Length == 2 && etimes.Length == 2)
                                {
                                    int.TryParse(stimes[0], out nshour);
                                    int.TryParse(stimes[1], out nsminute);
                                    int.TryParse(etimes[0], out nehour);
                                    int.TryParse(etimes[1], out neminute);

                                    nstime = nshour * 60 + nsminute;
                                    netime = nehour * 60 + neminute;
                                }
                            }

                            if (weekdays == "매일")
                            {
                                mon_open = tue_open = wed_open = thu_open = fri_open = sat_open = sun_open = "1";
                            }
                            else if (weekdays == "평일")
                            {
                                mon_open = tue_open = wed_open = thu_open = fri_open = "1";
                            }
                            else if (weekdays == "토요일")
                            {
                                sat_open = "1";
                            }
                            else if (weekdays == "일요일")
                            {
                                sun_open = "1";
                            }
                            else if (weekdays == "평일, 토요일")
                            {
                                mon_open = tue_open = wed_open = thu_open = fri_open = sat_open = "1";
                            }
                            else if (weekdays == "평일, 일요일")
                            {
                                mon_open = tue_open = wed_open = thu_open = fri_open = sun_open = "1";
                            }
                            else if (weekdays == "토요일, 일요일")
                            {
                                sat_open = sun_open = "1";
                            }

                            if (sun_open == "0" && tue_open == "0" && wed_open == "0" && thu_open == "0" && fri_open == "0" && sat_open == "0" && sun_open == "0")
                            {
                                continue;
                            }
                            strqry = "INSERT INTO TB_SHOP_OPERTIME(shop_cd,shop_time_begin,shop_time_end,shop_mon_open,shop_tue_open,shop_wed_open,shop_thu_open,shop_fri_open,shop_sat_open,shop_sun_open) ";
                            strqry += "VALUES (" + shop_cd + "," + nstime.ToString() + "," + netime.ToString() + "," + mon_open + "," + tue_open + "," + wed_open + "," + thu_open + "," + fri_open + "," + sat_open + "," + sun_open + ");";
                            SaveDBData(strqry);
                        }
                    }
                }

                //매장휴게시간 입력
                strqry = "DELETE FROM TB_SHOP_BREAKTIME WHERE shop_cd = " + shop_cd + ";";
                SaveDBData(strqry);

                if (Break_Tm_Info != "")
                {
                    string[] breaks = Break_Tm_Info.Split('\n');
                    for (int i = 0; i < breaks.Length; i++)
                    {

                        string mon_open = "0";
                        string tue_open = "0";
                        string wed_open = "0";
                        string thu_open = "0";
                        string fri_open = "0";
                        string sat_open = "0";
                        string sun_open = "0";
                        int nstime = 0;
                        int netime = 0;
                        int nshour = 0;
                        int nsminute = 0;
                        int nehour = 0;
                        int neminute = 0;
                        string stime = "";
                        string etime = "";
                        string time = breaks[i];
                        string[] times = time.Split('~');
                        if (times.Length == 2)
                        {
                            stime = times[0];
                            stime = stime.Replace("익일 밤", "오전").Replace("익일 새벽", "오전").Replace("새벽", "오전").Replace("밤", "오전").Replace("낮", "오후");
                            stime = stime.Trim();
                            etime = times[1];
                            etime = etime.Replace("익일 밤", "오전").Replace("익일 새벽", "오전").Replace("새벽", "오전").Replace("밤", "오전").Replace("낮", "오후");
                            etime = etime.Trim();

                            stime = DateTime.ParseExact(stime, "tt h:mm", null, System.Globalization.DateTimeStyles.AssumeLocal).ToString("HH:mm");
                            etime = DateTime.ParseExact(etime, "tt h:mm", null, System.Globalization.DateTimeStyles.AssumeLocal).ToString("HH:mm");

                            string[] stimes = stime.Split(':');
                            string[] etimes = etime.Split(':');
                            if (stimes.Length == 2 && etimes.Length == 2)
                            {
                                int.TryParse(stimes[0], out nshour);
                                int.TryParse(stimes[1], out nsminute);
                                int.TryParse(etimes[0], out nehour);
                                int.TryParse(etimes[1], out neminute);

                                nstime = nshour * 60 + nsminute;
                                netime = nehour * 60 + neminute;
                            }
                        }

                        mon_open = tue_open = wed_open = thu_open = fri_open = sat_open = sun_open = "1";

                        strqry = "INSERT INTO TB_SHOP_BREAKTIME(shop_cd,break_time_begin,break_time_end,break_mon_open,break_tue_open,break_wed_open,break_thu_open,break_fri_open,break_sat_open,break_sun_open) ";
                        strqry += "VALUES (" + shop_cd + "," + nstime.ToString() + "," + netime.ToString() + "," + mon_open + "," + tue_open + "," + wed_open + "," + thu_open + "," + fri_open + "," + sat_open + "," + sun_open + ");";
                        SaveDBData(strqry);
                    }
                }

                //정기휴일(매주 일요일, 연중무휴)
                strqry = "DELETE FROM TB_SHOP_REGULAR_HOLIDAY WHERE shop_cd = " + shop_cd + ";";
                SaveDBData(strqry);
                if (Close_Day != "연중무휴")
                {
                    string reg_hol_week = "";
                    string reg_hol_weekday = "";
                    if (Close_Day.IndexOf("매월") > -1 && Close_Day.IndexOf("마지막") > -1)
                    {
                        reg_hol_week = "6";
                    }
                    else if (Close_Day.IndexOf("매월") > -1 && Close_Day.IndexOf("다섯째") > -1)
                    {
                        reg_hol_week = "5";
                    }
                    else if (Close_Day.IndexOf("매월") > -1 && Close_Day.IndexOf("넷째") > -1)
                    {
                        reg_hol_week = "4";
                    }
                    else if (Close_Day.IndexOf("매월") > -1 && Close_Day.IndexOf("셋째") > -1)
                    {
                        reg_hol_week = "3";
                    }
                    else if (Close_Day.IndexOf("매월") > -1 && Close_Day.IndexOf("둘째") > -1)
                    {
                        reg_hol_week = "2";
                    }
                    else if (Close_Day.IndexOf("매월") > -1 && Close_Day.IndexOf("첫째") > -1)
                    {
                        reg_hol_week = "1";
                    }
                    else
                    {
                        reg_hol_week = "0";
                    }

                    if (Close_Day.IndexOf("일요일") > -1)
                    {
                        reg_hol_weekday = "0";
                    }
                    else if (Close_Day.IndexOf("월요일") > -1)
                    {
                        reg_hol_weekday = "1";
                    }
                    else if (Close_Day.IndexOf("화요일") > -1)
                    {
                        reg_hol_weekday = "2";
                    }
                    else if (Close_Day.IndexOf("수요일") > -1)
                    {
                        reg_hol_weekday = "3";
                    }
                    else if (Close_Day.IndexOf("목요일") > -1)
                    {
                        reg_hol_weekday = "4";
                    }
                    else if (Close_Day.IndexOf("금요일") > -1)
                    {
                        reg_hol_weekday = "5";
                    }
                    else if (Close_Day.IndexOf("토요일") > -1)
                    {
                        reg_hol_weekday = "6";
                    }

                    if (reg_hol_weekday != "")
                    {
                        strqry = "INSERT INTO TB_SHOP_REGULAR_HOLIDAY(shop_cd,reg_hol_week,reg_hol_weekday,reg_hol_info) ";
                        strqry += "VALUES (" + shop_cd + ",'" + reg_hol_week + "','" + reg_hol_weekday + "','" + Close_Day + "');";
                        SaveDBData(strqry);
                    }
                }


                if (!checkBox2.Checked)
                {

                    //메뉴를 지워라.
                    //TB_MENU_OPTION_GSEL 삭제
                    strqry = "DELETE T3 FROM TB_MENU_OPTION_GSEL AS T3 INNER JOIN TB_MENU_LIST AS T1 ON T3.menu_cd = T1.menu_cd INNER JOIN TB_MENU_GROUP AS T2 ON T1.menu_group_cd = T2.menu_group_cd WHERE T2.shop_cd = " + shop_cd + ";";
                    SaveDBData(strqry);
                    //TB_MENU_PRICE 삭제
                    strqry = "DELETE T3 FROM TB_MENU_PRICE AS T3 INNER JOIN TB_MENU_LIST AS T1 ON T3.menu_cd = T1.menu_cd INNER JOIN TB_MENU_GROUP AS T2 ON T1.menu_group_cd = T2.menu_group_cd WHERE T2.shop_cd = " + shop_cd + ";";
                    SaveDBData(strqry);
                    //TB_MENU_LIST 삭제
                    strqry = "DELETE T1 FROM TB_MENU_LIST AS T1 INNER JOIN TB_MENU_GROUP AS T2 ON T1.menu_group_cd = T2.menu_group_cd WHERE T2.shop_cd = " + shop_cd + ";";
                    SaveDBData(strqry);
                    //TB_MENU_OPTION 삭제
                    strqry = "DELETE T1 FROM TB_MENU_OPTION AS T1 INNER JOIN TB_MENU_OPTION_GROUP AS T2 ON T1.option_group_cd = T2.option_group_cd WHERE T2.shop_cd = " + shop_cd + ";";
                    SaveDBData(strqry);
                    //TB_MENU_OPTION_GROUP 삭제
                    strqry = "DELETE FROM TB_MENU_OPTION_GROUP WHERE shop_cd = " + shop_cd + ";";
                    SaveDBData(strqry);
                    //TB_MENU_GROUP 삭제
                    strqry = "DELETE FROM TB_MENU_GROUP WHERE shop_cd = " + shop_cd + ";";
                    SaveDBData(strqry);


                    //여기서 옵션그룹/옵션을 저장하고
                    foreach (KeyValuePair<string, Option_Group> kvp in d_ot_group_list)
                    {
                        string tmp_option_group_cd = kvp.Key;
                        Option_Group option_group = kvp.Value;
                        string tmp_option_gcd = option_group.tmp_option_group_cd;
                        string option_group_name = option_group.option_group_name;

                        strqry = "INSERT INTO TB_MENU_OPTION_GROUP(shop_cd, option_group_name, option_group_regdt, option_group_del, tmp_option_group_cd) ";
                        strqry += "VALUES (" + shop_cd + ", '" + option_group_name + "', NOW(), 0, " + tmp_option_gcd + ");";
                        string option_gcd = SaveDBData(strqry); //저장된 옵션그룹 CD

                        List<Option> option_list = option_group.options;
                        int option_sort = 0;
                        for (int i = 0; i < option_list.Count; i++)
                        {
                            Option ot = option_list[i];
                            string tmp_option_ggcd = ot.tmp_option_group_cd;
                            string tmp_option_cd = ot.tmp_option_cd;
                            string option_name = ot.option_name;
                            string option_amount = ot.option_amount;
                            string option_hide = ot.option_hide;
                            string option_soldout = ot.option_soldout;
                            string option_gubun = ot.option_gubun;
                            option_sort++;

                            strqry = "INSERT INTO TB_MENU_OPTION(option_group_cd, option_name, option_amount, option_hide, option_soldout, option_sort, option_regdt, option_del, tmp_option_group_cd, tmp_option_cd) ";
                            strqry += "VALUES (" + option_gcd + ", '" + option_name + "', " + option_amount + ", " + option_hide + ", " + option_soldout + ", " + option_sort.ToString() + ", NOW(), 0, " + tmp_option_ggcd + ", " + tmp_option_cd + ");";
                            string option_cd = SaveDBData(strqry); //저장된 옵션 CD
                        }
                    }

                    //여기서 메뉴그룹을 저장하고
                    int menu_group_sort = 0;
                    foreach (KeyValuePair<string, Menu_Group> kvp in d_menu_group_list)
                    {
                        string tmp_group_cd = kvp.Key;
                        Menu_Group menu_group = kvp.Value;
                        string menu_group_name = menu_group.menu_group_name;
                        string menu_group_hide = menu_group.menu_group_hide;
                        string menu_group_soldout = menu_group.menu_group_soldout;
                        menu_group_sort++;
                        string menu_group_alcohol = menu_group.menu_group_alcohol;

                        strqry = "INSERT INTO TB_MENU_GROUP(shop_cd, menu_group_name, menu_group_hide, menu_group_soldout, menu_group_sort, menu_group_regdt, menu_group_del, menu_group_alcohol, tmp_group_cd, tmp_shop_cd) ";
                        strqry += "VALUES (" + shop_cd + ", '" + menu_group_name + "', '" + menu_group_hide + "', '" + menu_group_soldout + "', '" + menu_group_sort.ToString() + "', NOW(), 0, '" + menu_group_alcohol + "', '" + tmp_group_cd + "', '" + baeminShopcd + "');";
                        string menu_group_cd = SaveDBData(strqry);

                        List<Menu> menu_list = menu_group.menu;
                        for (int i = 0; i < menu_list.Count; i++)
                        {
                            Menu mn = menu_list[i];
                            string menu_image = mn.menu_image;
                            string menu_name = mn.menu_name;
                            menu_name = menu_name.Replace("'", "`");
                            string menu_main = mn.menu_main;
                            string menu_component = mn.menu_component;
                            menu_component = menu_component.Replace("'", "`");
                            string menu_description = mn.menu_description;
                            menu_description = menu_description.Replace("'", "`");
                            string menu_solout = mn.menu_solout;
                            string tmp_menu_cd = mn.tmp_menu_cd;
                            string menu_alcohol = mn.menu_alcohol;


                            if (menu_image != "")
                            {
                                //aws
                                DateTime span = DateTime.Now;
                                string fname = span.ToString(@"yyyyMMddHHmmssfff") + ".jpg";
                                SaveImageAws(shop_cd, menu_image, fname);
                                menu_image = "https://s3.baechelin.com/media/shop/" + shop_cd + "/" + fname;
                            }

                            strqry = "INSERT INTO TB_MENU_LIST(menu_group_cd, menu_name, menu_description, menu_component, menu_hide, menu_soldout, menu_soldout_date, menu_image, menu_main, menu_status, menu_sort, tmp_menu_cd, menu_alcohol) ";
                            strqry += "VALUES (" + menu_group_cd + ", '" + menu_name + "', '" + menu_description + "', '" + menu_component + "', 0, '" + menu_solout + "', null, '" + menu_image + "', '" + menu_main + "', 1, " + (i + 1).ToString() + ", '" + tmp_menu_cd + "', '" + menu_alcohol + "');";
                            string menu_cd = SaveDBData(strqry);

                            List<Menu_Price> menu_privce = mn.price;
                            for (int j = 0; j < menu_privce.Count; j++)
                            {
                                Menu_Price mp = menu_privce[j];
                                string menu_price_name = mp.menu_price_name;
                                string menu_price_amount = mp.menu_price_amount;
                                menu_price_name = menu_price_name.Replace("'", "`");

                                strqry = "INSERT INTO TB_MENU_PRICE(menu_cd, menu_price_name, menu_price_amount, tmp_menu_cd) ";
                                strqry += "VALUES (" + menu_cd + ", '" + menu_price_name + "', " + menu_price_amount + ", " + tmp_menu_cd + ");";
                                string price_cd = SaveDBData(strqry);
                            }

                            //여기서 옵션그룹코드를 리턴받아 메뉴와 연결한다.
                            List<Menu_Option_Group> menu_option_g = mn.options;
                            for (int j = 0; j < menu_option_g.Count; j++)
                            {
                                Menu_Option_Group mog = menu_option_g[j];
                                string tmp_option_group_cd = mog.tmp_option_group_cd;
                                string min_order_sel = mog.min_order_sel;
                                string min_order_qty = mog.min_order_qty;
                                string max_order_qty = mog.max_order_qty;

                                strqry = "INSERT INTO TB_MENU_OPTION_GSEL(option_group_cd, menu_cd, min_order_sel, min_order_qty, max_order_qty, option_g_name) ";
                                strqry += "SELECT option_group_cd, '" + menu_cd + "', '" + min_order_sel + "', '" + min_order_qty + "', '" + max_order_qty + "', option_group_name  FROM TB_MENU_OPTION_GROUP WHERE shop_cd = " + shop_cd + " AND tmp_option_group_cd = " + tmp_option_group_cd + " LIMIT 1;";
                                SaveDBData(strqry);
                            }
                        }
                    }
                }
                //여기서 매장을 연결한다.
                //저장

                strqry = "UPDATE TB_SHOP_REGIST_REQ SET shop_cd = " + shop_cd + " WHERE req_cd = " + req_cd + ";";
                SaveDBData(strqry);

                //strqry = "UPDATE TB_SHOP_BIZ_ACCOUNT SET biz_acc_cd = " + biz_acc_cd + " WHERE shop_cd = " + shop_cd + ";";
                strqry = "INSERT INTO TB_SHOP_BIZ_ACCOUNT(biz_acc_cd, shop_cd) VALUES (" + biz_acc_cd + ", " + shop_cd + ") ON DUPLICATE KEY UPDATE biz_acc_cd=" + biz_acc_cd + ";";

                SaveDBData(strqry);

                strqry = "UPDATE TB_SHOP_LIST SET biz_cd = " + biz_cd + " WHERE shop_cd = " + shop_cd + ";";
                SaveDBData(strqry);

                button3.Enabled = false;
                button4.Enabled = false;

                splash.CloseEnd();
                MessageBox.Show("정상등록 완료");
            }
            catch (Exception ex)
            {
                splash.CloseEnd();
                MessageBox.Show("오류 : " + ex.Message);
            }
        }
        #endregion

        #region -DB 저장
        private string SaveDBData(string pqry)
        {
            long lastId = 0;
            MySql.Data.MySqlClient.MySqlConnection myConn = new MySql.Data.MySqlClient.MySqlConnection(strConnMy);
            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();
            }

            try
            {
                string strqry = pqry;
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(strqry, myConn);
                cmd.ExecuteNonQuery();
                lastId = cmd.LastInsertedId;
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }

            return lastId.ToString();
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            string bizcd = "";
            bizcd = textBox1.Text;
            bizcd = bizcd.Trim();
            if (bizcd == "")
            {
                MessageBox.Show("사업자등록번호를 입력해주세요!");
                textBox1.Focus();
                return;
            }

            button3.Enabled = true;
            button4.Enabled = true;

            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            GetShopData(bizcd);
            GetShopDatareqdata(bizcd);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string strSelshop_cd = "";
            string strSelreqcd = "";
            string strSelbizno = "";
            string strSelzipcd = "";
            string strbiz_acc_cd = "";

            if (dataGridView2.CurrentRow == null)
            {
                MessageBox.Show("입점신청 매장을 선택해주세요.");
                return;
            }
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("매장을 선택해주세요.");
                return;
            }

            try
            {
                strSelshop_cd = dataGridView1.SelectedRows[0].Cells["shop_cd"].Value.ToString();
                strSelreqcd = dataGridView2.SelectedRows[0].Cells["req_cd"].Value.ToString();
                strSelbizno = dataGridView2.SelectedRows[0].Cells["biz_cd"].Value.ToString();
                strSelzipcd = dataGridView2.SelectedRows[0].Cells["req_zipcd"].Value.ToString();
                strbiz_acc_cd = dataGridView2.SelectedRows[0].Cells["biz_acc_cd"].Value.ToString();

                //저장
                string strqry = "UPDATE TB_SHOP_REGIST_REQ SET shop_cd = " + strSelshop_cd + " WHERE req_cd = " + strSelreqcd + ";";
                SaveDBData(strqry);

                string strqry2 = "UPDATE TB_SHOP_LIST SET biz_cd = " + strSelbizno + ", shop_zipcd = '" + strSelzipcd + "' WHERE shop_cd = " + strSelshop_cd + ";";
                SaveDBData(strqry2);

                string strqry3 = "UPDATE TB_SHOP_BIZ_ACCOUNT SET biz_acc_cd = " + strbiz_acc_cd + " WHERE shop_cd = " + strSelshop_cd + ";";
                SaveDBData(strqry3);

                MessageBox.Show("등록완료");
            }
            catch
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //실시간 배민데이터 가져오기
            dataGridView3.Rows.Clear();
            string strSelreqadddoro = "";

            if (dataGridView2.CurrentRow == null)
            {
                MessageBox.Show("입점신청 리스트에서 매장을 선택해주세요.");
                return;
            }
            try
            {
                strSelreqadddoro = dataGridView2.SelectedRows[0].Cells["req_add_doro"].Value.ToString();

                //주소로 좌표조회
                SScraping _sc = new SScraping();
                CookieCollection cookieCollection = new CookieCollection();
                CookieContainer cookieContainer = new CookieContainer();

                //매장상세정보
                string strUrl = "https://mapi.baechelin.com/mobile/api/v1/addresses?keyword=" + strSelreqadddoro;
                string strReturn = _sc.GetHtmlSource(strUrl, "utf-8", "", "", ref cookieCollection, ref cookieContainer);
                JavaScriptSerializer jss2 = new JavaScriptSerializer();
                dynamic data2 = jss2.Deserialize<dynamic>(strReturn);

                string admCd = "";
                string rnMgtSn = "";
                string udrtYn = "";
                string buldMnnm = "";
                string buldSlno = "";

                foreach (dynamic data3 in data2)
                {
                    admCd = data3["admCd"] == null ? "" : data3["admCd"].ToString();
                    rnMgtSn = data3["rnMgtSn"] == null ? "" : data3["rnMgtSn"].ToString();
                    udrtYn = data3["udrtYn"] == null ? "" : data3["udrtYn"].ToString();
                    buldMnnm = data3["buldMnnm"] == null ? "" : data3["buldMnnm"].ToString();
                    buldSlno = data3["buldSlno"] == null ? "" : data3["buldSlno"].ToString();
                    break;
                }

                //매장좌표
                strUrl = "https://mapi.baechelin.com/mobile/api/v1/addresses/geo-location?admCd=" + admCd + "&rnMgtSn=" + rnMgtSn + "&udrtYn=" + udrtYn + "&buldMnnm=" + buldMnnm + "&buldSlno=" + buldSlno;
                strReturn = _sc.GetHtmlSource(strUrl, "utf-8", "", "", ref cookieCollection, ref cookieContainer);
                JavaScriptSerializer jss4 = new JavaScriptSerializer();
                dynamic data5 = jss4.Deserialize<dynamic>(strReturn);
                string lat = data5["lat"] == null ? "" : data5["lat"].ToString();
                string lng = data5["lng"] == null ? "" : data5["lng"].ToString();

                GetBaeminShopList(lat, lng);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //실시간 배민데이터 가져오기
            //배민코드가 있으면 그걸로 조회한다.
            dataGridView3.Rows.Clear();
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("매장리스트에서 매장을 선택해주세요.");
                return;
            }
            try
            {
                string tmp_shop_cd = dataGridView1.SelectedRows[0].Cells["tmp_shop_cd"].Value == null ? "" : dataGridView1.SelectedRows[0].Cells["tmp_shop_cd"].Value.ToString();
                string lat = dataGridView1.SelectedRows[0].Cells["shop_lat"].Value == null ? "" : dataGridView1.SelectedRows[0].Cells["shop_lat"].Value.ToString();
                string lng = dataGridView1.SelectedRows[0].Cells["shop_lng"].Value == null ? "" : dataGridView1.SelectedRows[0].Cells["shop_lng"].Value.ToString();

                if (tmp_shop_cd != "")
                {
                    GetBaeminShopDetail(tmp_shop_cd, lat, lng);
                }
                else
                {
                    GetBaeminShopList(lat, lng);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //배민내용 > 배슐랭
            bool bImg = false;
            bool bMImg = false;
            if (chkImg.Checked)
            {
                //이미지 저장
                bImg = true;
            }
            if (checkBox1.Checked)
            {
                //이미지 저장
                bMImg = true;
            }
            if (dataGridView2.CurrentRow == null)
            {
                MessageBox.Show("입점신청리스트에서 매장을 선택해주세요.");
                return;
            }
            if (dataGridView3.CurrentRow == null)
            {
                MessageBox.Show("배민리스트에서 매장을 선택해주세요.");
                return;
            }
            try
            {
                string tmp_shop_cd = dataGridView3.SelectedRows[0].Cells["bm_shopcd"].Value == null ? "" : dataGridView3.SelectedRows[0].Cells["bm_shopcd"].Value.ToString();
                string lat = dataGridView3.SelectedRows[0].Cells["lat"].Value == null ? "" : dataGridView3.SelectedRows[0].Cells["lat"].Value.ToString();
                string lng = dataGridView3.SelectedRows[0].Cells["lng"].Value == null ? "" : dataGridView3.SelectedRows[0].Cells["lng"].Value.ToString();
                string logoUrl = dataGridView3.SelectedRows[0].Cells["logoUrl"].Value == null ? "" : dataGridView3.SelectedRows[0].Cells["logoUrl"].Value.ToString();

                if (tmp_shop_cd == "")
                {
                    MessageBox.Show("배민리스트에서 저장할 매장을 선택해주세요.");
                    return;
                }
                else
                {
                    GetBaeminShop(tmp_shop_cd, lat, lng, bImg, bMImg, logoUrl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //매장명으로 조회한다.
            splash = new SplashThread();
            dataGridView3.Rows.Clear();
            string strSelreqadddoro = "";
            string strreq_nm = "";

            if (dataGridView2.CurrentRow == null)
            {
                MessageBox.Show("입점신청 리스트에서 매장을 선택해주세요.");
                return;
            }
            try
            {
                splash.Open();
                strSelreqadddoro = textBox3.Text;

                //주소로 좌표조회
                SScraping _sc = new SScraping();
                CookieCollection cookieCollection = new CookieCollection();
                CookieContainer cookieContainer = new CookieContainer();

                //매장상세정보
                string strUrl = "https://mapi.baechelin.com/mobile/api/v1/addresses?keyword=" + strSelreqadddoro;
                string strReturn = _sc.GetHtmlSource(strUrl, "utf-8", "", "", ref cookieCollection, ref cookieContainer);
                JavaScriptSerializer jss2 = new JavaScriptSerializer();
                dynamic data2 = jss2.Deserialize<dynamic>(strReturn);

                string admCd = "";
                string rnMgtSn = "";
                string udrtYn = "";
                string buldMnnm = "";
                string buldSlno = "";

                foreach (dynamic data3 in data2)
                {
                    admCd = data3["admCd"] == null ? "" : data3["admCd"].ToString();
                    rnMgtSn = data3["rnMgtSn"] == null ? "" : data3["rnMgtSn"].ToString();
                    udrtYn = data3["udrtYn"] == null ? "" : data3["udrtYn"].ToString();
                    buldMnnm = data3["buldMnnm"] == null ? "" : data3["buldMnnm"].ToString();
                    buldSlno = data3["buldSlno"] == null ? "" : data3["buldSlno"].ToString();
                    break;
                }

                //매장좌표
                strUrl = "https://mapi.baechelin.com/mobile/api/v1/addresses/geo-location?admCd=" + admCd + "&rnMgtSn=" + rnMgtSn + "&udrtYn=" + udrtYn + "&buldMnnm=" + buldMnnm + "&buldSlno=" + buldSlno + "&jibunAddr=" + strSelreqadddoro;
                strReturn = _sc.GetHtmlSource(strUrl, "utf-8", "", "", ref cookieCollection, ref cookieContainer);
                JavaScriptSerializer jss4 = new JavaScriptSerializer();
                dynamic data5 = jss4.Deserialize<dynamic>(strReturn);
                string lat = data5["lat"] == null ? "" : data5["lat"].ToString();
                string lng = data5["lng"] == null ? "" : data5["lng"].ToString();

                string tmpShopnm = textBox2.Text;
                if (tmpShopnm != "")
                {
                    strreq_nm = tmpShopnm;
                }
                else
                {
                    strreq_nm = dataGridView2.SelectedRows[0].Cells["req_nm"].Value.ToString();
                }

                GetBaeminShopList_Keyword(strreq_nm, lat, lng);
                splash.CloseEnd();
            }
            catch (Exception ex)
            {
                splash.CloseEnd();
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //GetBaeminShop("13447810", "37.62081224", "127.1583642", false);
            GetBaeminShop("10000168", "37.53929192", "127.04668225", false, false, "");
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentRow == null)
            {
                MessageBox.Show("입점신청 리스트에서 매장을 선택해주세요.");
                return;
            }

            string strSelreqadddoro = dataGridView2.CurrentRow.Cells["req_add_doro"].Value.ToString();
            string strSelreqaddjibun = dataGridView2.CurrentRow.Cells["req_add_jibun"].Value == null ? "" : dataGridView2.CurrentRow.Cells["req_add_jibun"].Value.ToString();

            if (strSelreqaddjibun != "")
            {
                textBox3.Text = strSelreqaddjibun;
            }
            else
            {
                textBox3.Text = strSelreqadddoro;
            }
            textBox2.Text = dataGridView2.CurrentRow.Cells["req_nm"].Value.ToString();
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow == null)
            {
                MessageBox.Show("입점신청 리스트에서 매장을 선택해주세요.");
                return;
            }

            string strSelreqadddoro = dataGridView2.CurrentRow.Cells["req_add_doro"].Value.ToString();
            string strSelreqaddjibun = dataGridView2.CurrentRow.Cells["req_add_jibun"].Value == null ? "" : dataGridView2.CurrentRow.Cells["req_add_jibun"].Value.ToString();

            if (strSelreqaddjibun != "")
            {
                textBox3.Text = strSelreqaddjibun;
            }
            else
            {
                textBox3.Text = strSelreqadddoro;
            }
            textBox2.Text = dataGridView2.CurrentRow.Cells["req_nm"].Value.ToString();
        }

        private void button8_Click(object sender, EventArgs e) // 매장연결 오류 수정
        {
            splash = new SplashThread();
            splash.Open();

            string req_cd = dataGridView2.SelectedRows[0].Cells["req_cd"].Value.ToString();
            string biz_cd = dataGridView2.SelectedRows[0].Cells["biz_cd"].Value.ToString();
            string req_bizno = dataGridView2.SelectedRows[0].Cells["req_bizno"].Value.ToString();
            string req_zipcd = dataGridView2.SelectedRows[0].Cells["req_zipcd"].Value.ToString();
            string biz_acc_cd = dataGridView2.SelectedRows[0].Cells["biz_acc_cd"].Value.ToString();
            string new_type = dataGridView2.SelectedRows[0].Cells["new_type"].Value.ToString();
            string req_bankcd = dataGridView2.SelectedRows[0].Cells["req_bankcd"].Value.ToString();
            string req_acctno = dataGridView2.SelectedRows[0].Cells["req_acctno"].Value.ToString();
            string req_acctowner = dataGridView2.SelectedRows[0].Cells["req_acctowner"].Value.ToString();
            string req_acctimg = dataGridView2.SelectedRows[0].Cells["req_acctimg"].Value.ToString();
            string req_shop_cd = dataGridView2.SelectedRows[0].Cells["req_shop_cd"].Value.ToString();

            string strqry = "UPDATE TB_SHOP_LIST SET shop_status = '" + '3' + "' WHERE shop_cd <> " + req_shop_cd + " AND " + "shop_bizno= '" + req_bizno + "';";
            SaveDBData(strqry);

            splash.CloseEnd();

            MessageBox.Show("매장연결 오류 수정 완료");

            button8.Visible = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < dataGridView3.RowCount; i++)
            {
                dataGridView3.Rows[i].Visible = false;
            }

            String searchValue = textBox4.Text;
            int rowIndex = -1;
            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                if (row.Cells["bm_shopnm"].Value.ToString().Contains(searchValue))
                {
                    rowIndex = row.Index;
                    dataGridView3.Rows[rowIndex].Visible = true;
                }
            }
        }
    }
}
