using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFramework.Service
{
    public class UserHelper
    {

        /// <summary>
        /// 获取权限树
        /// </summary>
        /// <param name="id">当前角色</param>
        /// <returns></returns>
        public static ReturnData GetTreeData(string id)
        {
            DataContext context = new DataContext();
            
            var treeData = new List<TreeData>();

            var role = context.Roles.Single(x => x.ID == id);
            var AllAuthorityList = context.Authorities.ToList();
            var AuthorityList = AllAuthorityList.Where(x => x.ParentID == "0").ToList();
            foreach (var item in AuthorityList)
            {
                var parent = new TreeData
                {
                    title = item.ActionDesc,
                    id = item.ID,
                    field = "",
                    href = "",
                    children = new List<TreeData>(),
                    spread = true,
                    disabled = false
                };

                var childrenList = AllAuthorityList.Where(x => x.ParentID == item.ID).ToList();
                foreach (var item2 in childrenList)
                {
                    var children = new TreeData
                    {
                        title = item2.ActionDesc,
                        id = item2.ID,
                        field = "",
                        href = "",
                        children = new List<TreeData>(),
                        spread = true,
                        @checked = role.AuthorityID == null ? false : role.AuthorityID.Contains(item2.ID) ? true : false,
                        disabled = false
                    };
                    parent.children.Add(children);
                }
                treeData.Add(parent);
            }
            var result = new ReturnData(0,"",treeData);
            return result;
        }
    }
}
