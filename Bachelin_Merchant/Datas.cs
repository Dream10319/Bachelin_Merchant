using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bachelin_Merchant
{
    //메뉴그룹
    public class Menu_Group
    {
        public string menu_group_cd { get; set; } //메뉴그룹코드
        public string menu_group_name { get; set; }   //메뉴그룹명
        public string menu_group_alcohol { get; set; }   //주류여부(1:주류,0:아님)
        public string menu_group_hide { get; set; }   //메뉴그룹 숨김여부(1/0)
        public string menu_group_soldout { get; set; }   //메뉴그룹 품절여부(1/0) 
        public string gubun { get; set; }   //구분(1:insert, 2:update, 3:delete)
        public List<Menu> menu = new List<Menu>();   //메뉴
    }
    
    //메뉴
    public class Menu
    {
        public string menu_group_cd { get; set; } //메뉴그룹코드
        public string tmp_menu_cd { get; set; }   //배민메뉴코드
        public string menu_image { get; set; }   //메뉴이미지
        public string menu_name { get; set; }   //메뉴명
        public string menu_main { get; set; }   //대표메뉴등록(1:대표메뉴등록, 0:아님) 
        public string menu_component { get; set; }   //메뉴구성 
        public string menu_description { get; set; }   //메뉴설명
        public string menu_solout { get; set; }   //메뉴품절
        public string menu_alcohol { get; set; }   //주류여부(1:주류,0:아님)
        public List<Menu_Price> price = new List<Menu_Price>();   //메뉴 가격
        public List<Menu_Option_Group> options = new List<Menu_Option_Group>();   //메뉴_옵션그룹
    }

    //메뉴 가격
    public class Menu_Price
    {
        public string menu_price_name { get; set; } //메뉴가격명
        public string menu_price_amount { get; set; }   //메뉴가격
    }

    //메뉴_옵션그룹
    public class Menu_Option_Group
    {
        public string tmp_option_group_cd { get; set; } //배민옵션그룹코드
        public string min_order_sel { get; set; } //메뉴옵션필수여부
        public string min_order_qty { get; set; }   //최소갯수
        public string max_order_qty { get; set; }   //최대갯수
    }

    //옵션 그룹
    public class Option_Group
    {
        public string tmp_option_group_cd { get; set; } //배민옵션그룹코드
        public string option_group_name { get; set; }   //메뉴옵션그룹명
        public string gubun { get; set; }   //구분(1:insert, 2:update, 3:delete)
        public List<Option> options = new List<Option>();   //옵션
    }

    //옵션
    public class Option
    {
        public string tmp_option_group_cd { get; set; } //배민옵션그룹코드
        public string tmp_option_cd { get; set; } //배민옵션코드
        public string option_name { get; set; }   //메뉴옵션명
        public string option_amount { get; set; }   //메뉴옵션금액
        public string option_hide { get; set; }   //메뉴옵션숨김(1:숨김, 0:아님)
        public string option_soldout { get; set; }   //메뉴옵션품절(1:품절, 0:아님)
        public string option_gubun { get; set; }   //구분(1:insert, 2:update, 3:delete)
    }
}
