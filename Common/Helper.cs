//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-17    1.0        HDS         新建               
//-------------------------------------------------------------------------

#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


#endregion

namespace  Common
{
  public  class Helper
    {
        #region 在字符串两端添加指定的字符型引号和括号类符号
        /// <summary>
        /// 在字符串两端添加指定的字符型引号和括号类符号
        /// </summary>
        /// <param name="s"></param>
        /// <param name="quoter"></param>
        /// <returns></returns>
        public string InSertValues(string SertValues, char quoter)
        {
            char[] quoters = { '"', '\'', '(', '[', '@', '>' };
            if (!quoters.Contains(quoter))
                return SertValues;
            else
                switch (quoter)
                {
                    case '"':
                        return '"' + SertValues + '"';
                    case '\'':
                        return '\'' + SertValues + '\'';
                    case '(':
                        return '(' + SertValues + ')';
                    case '[':
                        return '[' + SertValues + ']';
                    case '@':
                        return '@' + SertValues;
                    case '>':
                        return '>' + SertValues;
                    default:
                        return SertValues;
                }
        }
        #endregion
    }
}
