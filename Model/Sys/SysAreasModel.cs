//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/06/23    1.0        MH        
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：SysAreasModel
 * 功能描述：区域基础表 实体类  
 * ******************************/
namespace Model.Sys
{
    public class SysAreasModel
    {

        ///summary
        ///位置id
        ///summary
        public int AreaId { get; set; }

        ///summary
        ///位置名称
        ///summary
        public string AreaName { get; set; }
        public string AreaPName { get; set; }
        ///summary
        ///父节点id
        ///summary
        public int ParentId { get; set; }

        ///summary
        ///排序
        ///summary
        public int Sort { get; set; }

        ///summary
        ///行政编码
        ///summary
        public int AreaCode { get; set; }

        ///summary
        ///状态：0-无效；1-有效
        ///summary
        public int State { get; set; }

        ///summary
        ///创建时间
        ///summary
        public DateTime CreateTime { get; set; }

    }

    public class VAreas
    {
        /// <summary>
        /// 位置id
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// 位置名称
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 父节点id
        /// </summary>
        public int ParentId { get; set; }

        

        /// <summary>
        /// 位置行政编码
        /// </summary>
        public string AreaCode { get; set; }

       
 

        /// <summary>
        /// 省ID
        /// </summary>
        public int area01 { get; set; }


        /// <summary>
        /// 省名称
        /// </summary>
        public string areaname01 { get; set; }


        /// <summary>
        /// 市ID
        /// </summary>
        public int area02 { get; set; }


        /// <summary>
        /// 市名称
        /// </summary>
        public string areaname02 { get; set; }


        /// <summary>
        /// 县ID
        /// </summary>
        public int area03 { get; set; }


        /// <summary>
        /// 县名称
        /// </summary>
        public string areaname03 { get; set; }


        /// <summary>
        /// 乡镇ID
        /// </summary>
        public int area04 { get; set; }


        /// <summary>
        /// 乡镇名称
        /// </summary>
        public string areaname04 { get; set; }

    }

}

