﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ReturnData
    {
        /// <summary>
        /// 代码
        /// </summary>
        public readonly int code;
        /// <summary>
        /// 消息
        /// </summary>
        public readonly string msg;
        /// <summary>
        /// 数据集合
        /// </summary>
        public readonly string data;
        /// <summary>
        /// 总条数
        /// </summary>
        public readonly int count;
        /// <summary>
        /// 当前页数
        /// </summary>
        public readonly int page;

        public ReturnData()
        {
            this.code = 200;
            this.msg = "";
            this.count = 0;
        }
        public ReturnData(int code)
        {
            this.code = (int)code;
            this.msg = "";
            this.count = 0;
        }
        public ReturnData(int code, string msg)
        {
            this.code = (int)code;
            this.msg = msg;
            this.count = 0;
        }
        public ReturnData(int code, string msg, string data)
        {
            this.code = (int)code;
            this.msg = msg;
            this.data = data;
            this.count = 0;
        }
        public ReturnData(int code, string msg, int count, string data)
        {
            this.code = (int)code;
            this.msg = msg;
            this.data = data;
            this.count = count;
        }
        public ReturnData(int code, string msg, int count, string data, int page)
        {
            this.code = (int)code;
            this.msg = msg;
            this.data = data;
            this.count = count;
            this.page = page;
        }
    }
    public enum CustomStatusCode
    {
        Success = 0, Error = 1, NotFound = 404, TimeOut = 1001
    }
}
