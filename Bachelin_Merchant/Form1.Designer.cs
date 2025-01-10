namespace Bachelin_Merchant
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.shop_cd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shop_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shop_bizno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shop_addr_doro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ct_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tmp_shop_cd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shop_lat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shop_lng = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.req_cd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.req_nm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.req_bizno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.req_add_doro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.biz_cd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.req_zipcd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.biz_acc_cd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.new_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.req_bankcd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.req_acctno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.req_acctowner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.req_acctimg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.req_add_jibun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.req_shop_cd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.bm_shopnm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bm_telno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bm_address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bm_cate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bm_shopcd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lng = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.logoUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.chkImg = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.button9 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(308, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "검색";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.MediumAquamarine;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.ColumnHeadersHeight = 30;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.shop_cd,
            this.shop_name,
            this.shop_bizno,
            this.shop_addr_doro,
            this.ct_name,
            this.tmp_shop_cd,
            this.shop_lat,
            this.shop_lng});
            this.dataGridView1.Location = new System.Drawing.Point(520, 9);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 20;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(57, 40);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.Visible = false;
            // 
            // shop_cd
            // 
            this.shop_cd.HeaderText = "매장코드";
            this.shop_cd.Name = "shop_cd";
            this.shop_cd.ReadOnly = true;
            this.shop_cd.Visible = false;
            // 
            // shop_name
            // 
            this.shop_name.HeaderText = "매장명";
            this.shop_name.Name = "shop_name";
            this.shop_name.ReadOnly = true;
            this.shop_name.Width = 150;
            // 
            // shop_bizno
            // 
            this.shop_bizno.HeaderText = "사업자번호";
            this.shop_bizno.Name = "shop_bizno";
            this.shop_bizno.ReadOnly = true;
            // 
            // shop_addr_doro
            // 
            this.shop_addr_doro.HeaderText = "주소";
            this.shop_addr_doro.Name = "shop_addr_doro";
            this.shop_addr_doro.ReadOnly = true;
            this.shop_addr_doro.Width = 200;
            // 
            // ct_name
            // 
            this.ct_name.HeaderText = "카테고리";
            this.ct_name.Name = "ct_name";
            this.ct_name.ReadOnly = true;
            // 
            // tmp_shop_cd
            // 
            this.tmp_shop_cd.HeaderText = "배민매장코드";
            this.tmp_shop_cd.Name = "tmp_shop_cd";
            this.tmp_shop_cd.ReadOnly = true;
            // 
            // shop_lat
            // 
            this.shop_lat.HeaderText = "위도";
            this.shop_lat.Name = "shop_lat";
            this.shop_lat.ReadOnly = true;
            this.shop_lat.Visible = false;
            // 
            // shop_lng
            // 
            this.shop_lng.HeaderText = "경도";
            this.shop_lng.Name = "shop_lng";
            this.shop_lng.ReadOnly = true;
            this.shop_lng.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(97, 20);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(206, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "2782701818";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "사업자등록번호";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(506, 243);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(105, 42);
            this.button2.TabIndex = 8;
            this.button2.Text = "^ 매장연결";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.MediumAquamarine;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView2.ColumnHeadersHeight = 30;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.req_cd,
            this.req_nm,
            this.req_bizno,
            this.req_add_doro,
            this.biz_cd,
            this.req_zipcd,
            this.biz_acc_cd,
            this.new_type,
            this.req_bankcd,
            this.req_acctno,
            this.req_acctowner,
            this.req_acctimg,
            this.req_add_jibun,
            this.req_shop_cd});
            this.dataGridView2.Location = new System.Drawing.Point(15, 94);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidth = 20;
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(759, 213);
            this.dataGridView2.TabIndex = 6;
            this.dataGridView2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            this.dataGridView2.SelectionChanged += new System.EventHandler(this.dataGridView2_SelectionChanged);
            // 
            // req_cd
            // 
            this.req_cd.HeaderText = "입점신청코드";
            this.req_cd.Name = "req_cd";
            this.req_cd.ReadOnly = true;
            this.req_cd.Visible = false;
            // 
            // req_nm
            // 
            this.req_nm.HeaderText = "매장명";
            this.req_nm.Name = "req_nm";
            this.req_nm.ReadOnly = true;
            this.req_nm.Width = 200;
            // 
            // req_bizno
            // 
            this.req_bizno.HeaderText = "사업자번호";
            this.req_bizno.Name = "req_bizno";
            this.req_bizno.ReadOnly = true;
            this.req_bizno.Width = 150;
            // 
            // req_add_doro
            // 
            this.req_add_doro.HeaderText = "주소";
            this.req_add_doro.Name = "req_add_doro";
            this.req_add_doro.ReadOnly = true;
            this.req_add_doro.Width = 250;
            // 
            // biz_cd
            // 
            this.biz_cd.HeaderText = "사업장코드";
            this.biz_cd.Name = "biz_cd";
            this.biz_cd.ReadOnly = true;
            this.biz_cd.Visible = false;
            // 
            // req_zipcd
            // 
            this.req_zipcd.HeaderText = "우편번호";
            this.req_zipcd.Name = "req_zipcd";
            this.req_zipcd.ReadOnly = true;
            this.req_zipcd.Visible = false;
            // 
            // biz_acc_cd
            // 
            this.biz_acc_cd.HeaderText = "계정코드";
            this.biz_acc_cd.Name = "biz_acc_cd";
            this.biz_acc_cd.ReadOnly = true;
            this.biz_acc_cd.Visible = false;
            // 
            // new_type
            // 
            this.new_type.HeaderText = "신규타입";
            this.new_type.Name = "new_type";
            this.new_type.ReadOnly = true;
            this.new_type.Visible = false;
            // 
            // req_bankcd
            // 
            this.req_bankcd.HeaderText = "정산계좌은행코드";
            this.req_bankcd.Name = "req_bankcd";
            this.req_bankcd.ReadOnly = true;
            this.req_bankcd.Visible = false;
            // 
            // req_acctno
            // 
            this.req_acctno.HeaderText = "정산계좌계좌번호";
            this.req_acctno.Name = "req_acctno";
            this.req_acctno.ReadOnly = true;
            this.req_acctno.Visible = false;
            // 
            // req_acctowner
            // 
            this.req_acctowner.HeaderText = "정산계좌예금주명";
            this.req_acctowner.Name = "req_acctowner";
            this.req_acctowner.ReadOnly = true;
            this.req_acctowner.Visible = false;
            // 
            // req_acctimg
            // 
            this.req_acctimg.HeaderText = "정산계좌통장사본";
            this.req_acctimg.Name = "req_acctimg";
            this.req_acctimg.ReadOnly = true;
            this.req_acctimg.Visible = false;
            // 
            // req_add_jibun
            // 
            this.req_add_jibun.HeaderText = "지번주소";
            this.req_add_jibun.Name = "req_add_jibun";
            this.req_add_jibun.Width = 250;
            // 
            // req_shop_cd
            // 
            this.req_shop_cd.HeaderText = "연결된매장코드";
            this.req_shop_cd.Name = "req_shop_cd";
            this.req_shop_cd.ReadOnly = true;
            this.req_shop_cd.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "입점신청리스트";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(459, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "매장리스트";
            this.label3.Visible = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(619, 39);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(132, 42);
            this.button3.TabIndex = 11;
            this.button3.Text = "-> 배민데이터 조회";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 386);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "배민리스트";
            // 
            // dataGridView3
            // 
            this.dataGridView3.AllowUserToAddRows = false;
            this.dataGridView3.AllowUserToDeleteRows = false;
            this.dataGridView3.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.MediumAquamarine;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView3.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView3.ColumnHeadersHeight = 30;
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.bm_shopnm,
            this.bm_telno,
            this.bm_address,
            this.bm_cate,
            this.bm_shopcd,
            this.lat,
            this.lng,
            this.logoUrl});
            this.dataGridView3.Location = new System.Drawing.Point(17, 402);
            this.dataGridView3.MultiSelect = false;
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowHeadersWidth = 20;
            this.dataGridView3.RowTemplate.Height = 23;
            this.dataGridView3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView3.Size = new System.Drawing.Size(758, 174);
            this.dataGridView3.TabIndex = 13;
            // 
            // bm_shopnm
            // 
            this.bm_shopnm.HeaderText = "매장명";
            this.bm_shopnm.Name = "bm_shopnm";
            this.bm_shopnm.ReadOnly = true;
            this.bm_shopnm.Width = 150;
            // 
            // bm_telno
            // 
            this.bm_telno.HeaderText = "전화번호";
            this.bm_telno.Name = "bm_telno";
            this.bm_telno.ReadOnly = true;
            // 
            // bm_address
            // 
            this.bm_address.HeaderText = "주소";
            this.bm_address.Name = "bm_address";
            this.bm_address.ReadOnly = true;
            this.bm_address.Width = 200;
            // 
            // bm_cate
            // 
            this.bm_cate.HeaderText = "카테고리";
            this.bm_cate.Name = "bm_cate";
            this.bm_cate.ReadOnly = true;
            // 
            // bm_shopcd
            // 
            this.bm_shopcd.HeaderText = "배민매장코드";
            this.bm_shopcd.Name = "bm_shopcd";
            this.bm_shopcd.ReadOnly = true;
            // 
            // lat
            // 
            this.lat.HeaderText = "위도";
            this.lat.Name = "lat";
            this.lat.Visible = false;
            // 
            // lng
            // 
            this.lng.HeaderText = "경도";
            this.lng.Name = "lng";
            this.lng.Visible = false;
            // 
            // logoUrl
            // 
            this.logoUrl.HeaderText = "매장로고";
            this.logoUrl.Name = "logoUrl";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(675, 314);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(99, 42);
            this.button4.TabIndex = 14;
            this.button4.Text = "매장업데이트 ^";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(414, 39);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(132, 42);
            this.button5.TabIndex = 15;
            this.button5.Text = "-> 배민데이터 조회";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Visible = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(227, 310);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(102, 47);
            this.button6.TabIndex = 16;
            this.button6.Text = "V 배민데이터 조회";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(619, 0);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(132, 42);
            this.button7.TabIndex = 17;
            this.button7.Text = "test";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Visible = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(59, 334);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(163, 20);
            this.textBox2.TabIndex = 18;
            // 
            // chkImg
            // 
            this.chkImg.AutoSize = true;
            this.chkImg.Location = new System.Drawing.Point(574, 327);
            this.chkImg.Name = "chkImg";
            this.chkImg.Size = new System.Drawing.Size(106, 17);
            this.chkImg.TabIndex = 19;
            this.chkImg.Text = "메뉴이미지 저장";
            this.chkImg.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(470, 327);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(106, 17);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "매장이미지 저장";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(367, 327);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(106, 17);
            this.checkBox2.TabIndex = 21;
            this.checkBox2.Text = "매장정보만 저장";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(59, 310);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(163, 20);
            this.textBox3.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 320);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "주소";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 343);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "매장명";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(367, 363);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(111, 32);
            this.button8.TabIndex = 25;
            this.button8.Text = "매장연결 오류 수정";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Visible = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(580, 370);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(113, 20);
            this.textBox4.TabIndex = 26;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(699, 369);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 23);
            this.button9.TabIndex = 27;
            this.button9.Text = "검색";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 595);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.chkImg);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.dataGridView3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "매장연결 (2021-10-21 #1)";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.DataGridViewTextBoxColumn shop_cd;
        private System.Windows.Forms.DataGridViewTextBoxColumn shop_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn shop_bizno;
        private System.Windows.Forms.DataGridViewTextBoxColumn shop_addr_doro;
        private System.Windows.Forms.DataGridViewTextBoxColumn ct_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn tmp_shop_cd;
        private System.Windows.Forms.DataGridViewTextBoxColumn shop_lat;
        private System.Windows.Forms.DataGridViewTextBoxColumn shop_lng;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.DataGridViewTextBoxColumn req_cd;
        private System.Windows.Forms.DataGridViewTextBoxColumn req_nm;
        private System.Windows.Forms.DataGridViewTextBoxColumn req_bizno;
        private System.Windows.Forms.DataGridViewTextBoxColumn req_add_doro;
        private System.Windows.Forms.DataGridViewTextBoxColumn biz_cd;
        private System.Windows.Forms.DataGridViewTextBoxColumn req_zipcd;
        private System.Windows.Forms.DataGridViewTextBoxColumn biz_acc_cd;
        private System.Windows.Forms.DataGridViewTextBoxColumn new_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn req_bankcd;
        private System.Windows.Forms.DataGridViewTextBoxColumn req_acctno;
        private System.Windows.Forms.DataGridViewTextBoxColumn req_acctowner;
        private System.Windows.Forms.DataGridViewTextBoxColumn req_acctimg;
        private System.Windows.Forms.DataGridViewTextBoxColumn req_add_jibun;
        private System.Windows.Forms.DataGridViewTextBoxColumn req_shop_cd;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.CheckBox chkImg;
        private System.Windows.Forms.DataGridViewTextBoxColumn bm_shopnm;
        private System.Windows.Forms.DataGridViewTextBoxColumn bm_telno;
        private System.Windows.Forms.DataGridViewTextBoxColumn bm_address;
        private System.Windows.Forms.DataGridViewTextBoxColumn bm_cate;
        private System.Windows.Forms.DataGridViewTextBoxColumn bm_shopcd;
        private System.Windows.Forms.DataGridViewTextBoxColumn lat;
        private System.Windows.Forms.DataGridViewTextBoxColumn lng;
        private System.Windows.Forms.DataGridViewTextBoxColumn logoUrl;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button button9;
    }
}

