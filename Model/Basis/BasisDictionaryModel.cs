//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/05/30    1.0        ZBB        新建        
//-------------------------------------------------------------------------
#region 参数
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：BasisDictionaryModel
 * 功能描述：系统字典表 实体类  
 * ******************************/
namespace Model.Basis
{
    public class BasisDictionaryModel
    {
        /// <summary>
        /// 字典id
        /// </summary>
        public int DictionaryId { get; set; }

        /// <summary>
        /// 字典名称
        /// </summary>
        public string DictionaryName { get; set; }

        /// <summary>
        /// 字典序号
        /// </summary>
        public string DictionaryNumber { get; set; }

        /// <summary>
        /// 字典类型:0-供应商类别;1-供应商种类;2-合作层级;3-供应商服务类型;4-供应商状态;5-仓储供应商附件;
        ///6-仓储证件类型;7-培养期望;8-通知类型;9-企业性质;10-配送线路类型;11-不合格品类型
        /// </summary>
        public int DictionaryType { get; set; }

        /// <summary>
        /// 字典排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 字典状态:0-无效;1-有效
        /// </summary>
        public int DictionaryState { get; set; }

        /// <summary>
        /// 显示状态
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// 使用类型：0-系统统一;1-公司使用
        /// </summary>
        public int UseType { get; set; }

        /// <summary>
        /// 显示字典类型名称
        /// </summary>
        public string DictionaryTypeName { get; set; }

        /// <summary>
        /// 使用使用类型名称
        /// </summary>
        public string UseTypeName { get; set; }

        /// <summary>
        /// 系统公司id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 使用使用类型名称
        /// </summary>
        public string CompanyName { get; set; }

    }

    /// <summary>
    /// 解决字典类型为4的数据问题
    /// </summary>
    public class Dicts
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
    }
}
