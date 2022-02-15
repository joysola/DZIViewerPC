using DST.Database.Model;
using DST.Database.Model.DictModel;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using Nico.Common;
using System.Collections.Generic;

namespace DST.ApiClient.Api
{
    class DictApi : BaseApi<DictApi>
    {
        /// <summary>
        /// 根据键值获取字典(注意每次返回一个数组,但是实际上只会取第一项)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Url("dst-system/system/dict/listTreeByCode")]
        [HttpGet]
        internal ApiResponse<List<DictModel>> GetDict(string code) => GetResult();

        /// <summary>
        /// 获取共建医院信息
        /// </summary>
        /// <returns></returns>
        //[Url("dst-fund/fund/hospital/pageByHospital")]
        [Url("dst-fund/fund/hospital/client/pageByHospital")]
        [HttpPost]
        internal ApiResponse<HospitalReturnModel> PageByHospital([PostContent] HospitalModel hospitalDto, int current, int size) => GetResult();

        /// <summary>
        /// 获取该院所有送检医生
        /// </summary>
        /// <returns></returns>
        [Url("api/deepsight-system/user/listDoctorByLoginId")]
        [HttpGet]
        internal ApiResponse<List<SubmitDoctorModel>> GetSubmitDoctors() => GetResult();

        /// <summary>
        /// 获取检验项目字典
        /// </summary>
        /// <returns></returns>
        [Url("dst-fund/fund/product/listDetailsByHospitalId")]
        [HttpGet]
        internal ApiResponse<List<ProductModel>> GetProductModels(string hospitalId) => GetResult();
        /// <summary>
        /// 获取C端检验项目字典
        /// </summary>
        /// <returns></returns>
        [Url("dst-fund/fund/product/listDetailsByCurrentLoginHospital")]
        [HttpGet]
        internal ApiResponse<List<ProductModel>> GetCSProductModels() => GetResult();
        
        /// <summary>
        /// 根据医院获取医生信息
        /// </summary>
        /// <returns></returns>
        [Url("dst-fund/fund/doctor/listByHospitalId")]
        [HttpGet]
        internal ApiResponse<List<DoctorInfoModel>> ListByHospitalId(string hospitalId) => GetResult();
    }
}