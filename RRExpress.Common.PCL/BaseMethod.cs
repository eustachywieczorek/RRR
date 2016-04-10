﻿using Newtonsoft.Json;
using RRExpress.Common.Exceptions;
using RRExpress.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RRExpress.Common {
    /// <summary>
    /// API方法基类
    /// </summary>
    public abstract class BaseMethod {

        /// <summary>
        /// 是否有错误
        /// </summary>
        public bool HasError {
            get {
                return this.ErrorType != null;
            }
        }

        /// <summary>
        /// 错误类型
        /// </summary>
        public ErrorTypes? ErrorType {
            get; set;
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg {
            get;
            set;
        }

        /// <summary>
        /// 具体的 Client 类型
        /// </summary>
        public abstract Type ClientSetupType {
            get;
        }

        /// <summary>
        /// 请求地址
        /// </summary>
        public abstract string Module {
            get;
        }

        /// <summary>
        /// 沙箱环境的请求地址
        /// </summary>
        public virtual string SandboxModule {
            get {
                return this.Module;
            }
        }



        /// <summary>
        /// 验证数据（虚方法，在子类中重写验证）
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<string> InnerValidate() {
            return Enumerable.Empty<string>();
        }

        ///// <summary>
        ///// 检查输入参数的合法性
        ///// </summary>
        ///// <returns></returns>
        //public ValidationResults Validate() {
        //    var validator = ValidationFactory.CreateValidator(this.GetType());
        //    var results = validator.Validate(this);

        //    var msgs = this.InnerValidate();
        //    if (msgs != null) {
        //        results.AddAllResults(msgs.Where(m => !string.IsNullOrWhiteSpace(m)).Select(m => new ValidationResult(m, this, "", "", null)));
        //    }
        //    return results;
        //}


        /// <summary>
        /// 输出请求内容
        /// </summary>
        /// <returns></returns>
        public abstract Task<string> ExportRequestData();


        /// <summary>
        /// 获取执行结果的字符串
        /// </summary>
        /// <param name="client"></param>
        /// <param name="moduleUrl"></param>
        /// <returns></returns>
        protected abstract Task<Tuple<byte[], HttpStatusCode>> GetResult(IClientSetup client, string moduleUrl);
    }



    public abstract class BaseMethod<T> : BaseMethod {

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="setup"></param>
        /// <param name="useSandBox">是否使用沙箱环境</param>
        /// <returns></returns>
        public async Task<T> Execute(ApiClientOption option, IClientSetup setup) {
            var url = setup.GetUrl(this, option.UseSandbox);
            var result = await this.GetResult(setup, url);

            if (result == null || result.Item1 == null) {
                return default(T);
            }

            var status = result.Item2;
            var error = status.Convert();

            if (error.HasValue) {
                throw new MethodRequestException() {
                    StateCode = status,
                    ErrorType = error.Value
                };
            }

            //解析返回结果中，供应商自定义的错误信息
            var msg = await this.GetErrorMessageFromResult(result.Item1);
            if (!string.IsNullOrWhiteSpace(msg)) {
                throw new ContentWithErrorException(msg);
            } else {
                try {
                    //解析结果
                    return await this.Parse(setup, result.Item1);
                } catch {
                    throw new ParseException() {
                        TargetType = typeof(T),
                        TargetData = result.Item1
                    };
                }
            }
        }

        /// <summary>
        /// 将返回字符串解析为结果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected abstract Task<T> Parse(IClientSetup setup, byte[] result);


        /// <summary>
        /// 解析返回结果中的错误信息
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual Task<string> GetErrorMessageFromResult(byte[] result) {
            return Task.FromResult<string>(null);
        }
    }
}