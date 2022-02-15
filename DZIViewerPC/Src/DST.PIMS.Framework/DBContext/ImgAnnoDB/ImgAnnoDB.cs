using DST.Database.Model;
using DST.PIMS.Framework.Controls;
using DST.PIMS.Framework.ExtendContext;
using Nico.Common;
using Snowflake.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.DBContext.ImgAnnoDB
{
    public class ImgAnnoDB
    {
        public static ImgAnnoDB Instance { get; } = new ImgAnnoDB();
        private readonly LocalDBContext _context = LocalDB.Context;
        private readonly IdWorker _idWorker = LocalDB.SnowflakeId;

        /// <summary>
        /// 根据样本id获取所有标记
        /// </summary>
        /// <param name="sampleId"></param>
        /// <returns></returns>
        public List<Img_Anno> GetImgAnnoListbySampleId(string sampleId)
        {
            var result = new List<Img_Anno>();
            var tmp = _context.Set<Img_Anno>().Where(x => x.Sample_Id == sampleId).ToList();
            return tmp ?? result;
        }

        public bool AddImgAnno(Img_Anno anno)
        {
            var result = false;
            if (!string.IsNullOrEmpty(anno?.Sample_Id) && string.IsNullOrEmpty(anno?.Id))
            {
                anno.Id = _idWorker.NextId().ToString();
                anno.Create_Time = DateTime.Now;
                anno.Create_User = ExtendAppContext.Current.LoginModel.User_Name;

                _context.Set<Img_Anno>().Add(anno);
                var changeRows = _context.SaveChanges();
                result = changeRows == 1;
            }
            return result;
        }

        public bool UpdateImgAnno(Img_Anno anno)
        {
            var result = false;
            if (!string.IsNullOrEmpty(anno?.Id))
            {
                anno.Update_User = ExtendAppContext.Current.LoginModel.User_Name;
                anno.Update_Time = DateTime.Now;
                _context.Entry(anno).State = EntityState.Modified;
                var changeRows = _context.SaveChanges();
                result = changeRows == 1;
            }
            return result;
        }

        public bool DeleteImgAnno(Img_Anno anno)
        {
            var result = false;
            if (!string.IsNullOrEmpty(anno?.Id))
            {
                _context.Set<Img_Anno>().Remove(anno);
                var changeRows = _context.SaveChanges();
                result = changeRows == 1;
            }
            return result;
        }

        /// <summary>
        /// 保存所有标记
        /// </summary>
        /// <param name="newList"></param>
        /// <param name="oldList"></param>
        /// <param name="sampleId"></param>
        /// <returns></returns>
        public bool SaveAnnoList(IEnumerable<AnnoBase> newList, IEnumerable<Img_Anno> oldList, string sampleId)
        {
            var result = false;
            var changeRows = 0;


            var addBaseList = newList?.Where(x => string.IsNullOrEmpty(x.Id)).ToList();

            var updateBaseList = new List<AnnoBase>();
            var existOldList = new List<Img_Anno>();
            var tmpList = newList.Except(addBaseList); // 新集合中存在id的项目

            // 1. 获取历史里存在，但是新的集合中不存在的，用以删除
            oldList.Join(tmpList, o => o.Id, n => n.Id, (o, n) =>
            {
                existOldList.Add(o);
                updateBaseList.Add(n);
                return n;
            }).ToList();

            var deleteList = oldList.Except(existOldList).ToList();

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    // 1. 删除
                    var deleteIds = deleteList.Select(x => x.Id);
                    var dels = _context.Set<Img_Anno>().Where(x => deleteIds.Contains(x.Id));
                    _context.Set<Img_Anno>().RemoveRange(dels);
                    if (deleteList.Count > 0)
                    {
                        changeRows += _context.SaveChanges();
                    }



                    // 2. 新增
                    foreach (var anno in addBaseList)
                    {
                        var imgAnno = new Img_Anno
                        {
                            Id = _idWorker.NextId().ToString(),
                            Anno_Name = anno.Anno_Name, // 标记名称
                            Start_Point_X = anno.CurStart.X,
                            Start_Point_Y = anno.CurStart.Y,
                            End_Point_X = anno.CurEnd.X,
                            End_Point_Y = anno.CurEnd.Y,
                            Sample_Id = sampleId,
                            Description = anno.Description, // 描述
                            ThumbImg = anno.ThumbImg,
                            Create_Time = DateTime.Now,
                            Create_User = ExtendAppContext.Current.LoginModel.User_Name,
                        };
                        _context.Set<Img_Anno>().Add(imgAnno);
                    }
                    if (addBaseList.Count > 0)
                    {
                        changeRows += _context.SaveChanges();
                    }


                    // 2. 更新
                    var updateIds = updateBaseList.Select(x => x.Id);
                    var upls = _context.Set<Img_Anno>().Where(x => updateIds.Contains(x.Id)).ToList();

                    _ = upls.Join(updateBaseList, n => n.Id, o => o.Id, (n, o) =>
                    {
                        n.Start_Point_X = o.CurStart.X;
                        n.Start_Point_Y = o.CurStart.Y;
                        n.End_Point_X = o.CurEnd.X;
                        n.End_Point_Y = o.CurEnd.Y;
                        n.Anno_Name = o.Anno_Name;
                        n.Description = o.Description;
                        n.Update_User = ExtendAppContext.Current.LoginModel.User_Name;
                        n.Update_Time = DateTime.Now;
                        return n;
                    }).ToList();
                    if (updateBaseList.Count > 0)
                    {
                        changeRows += _context.SaveChanges();
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    Logger.Error("更新失败！", ex);
                    trans.Rollback();
                }
            }

            result = addBaseList.Count + deleteList.Count + updateBaseList.Count == changeRows;
            return result;
        }
    }
}
