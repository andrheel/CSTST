using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSTST.Models;
using CSTST.Data;

namespace CSTST.Controllers
{
    public partial class HomeController : Controller
    {
        public IActionResult resetdb() => safe(() =>
        {
            dal.ResetDB();
            return OK("OK");
        });


        public IActionResult statdb() => safe(() => Data(dal.StatDB()));


        #region User Api

        async public Task<IActionResult> getallusers() => await safe(async () =>
            Data(await (dal as IUserDAL).All()));

        [HttpPost]
        async public Task<IActionResult> getuser([FromBody] ReqData<int> data) => await safe(async () =>
        {
            var usr = await (dal as IUserDAL).Find(data.Value);
            var belongto = await (dal as IGroupDAL).FindByUserId(data.Value);
            return Data(new { usr, belongto });
        });


        [HttpPost]
        async public Task<IActionResult> createuser([FromBody] ReqData<string> data) => await safe(async () =>
        {
            (dal as IUserDAL).Create(data.Value);
            return await getallusers();
        });


        [HttpPost]
        async public Task<IActionResult> deleteuser([FromBody] ReqData<int> data) => await safe(async () =>
        {
            (dal as IUserDAL).Delete(data.Value);
            return await getallusers();
        });


        [HttpPost]
        async public Task<IActionResult> linkusergroup([FromBody] ReqData<KeyValuePair<int, string>> data) => await safe(async () =>
        {
            var (userId, grpName) = (data.Value.Key, data.Value.Value);
            var grp = await (dal as IGroupDAL).FindByName(grpName);
            if (grp == null) throw new Exception($"Group `{grpName}` not found");
            (dal as IUserDAL).Link(grp.Id, userId);
            return await getuser(new ReqData<int>(userId));
        });


        [HttpPost]
        async public Task<IActionResult> unlinkusergroup([FromBody] ReqData<KeyValuePair<int, int>> data) => await safe(async () =>
        {
            var (userId, grpId) = (data.Value.Key, data.Value.Value);
            (dal as IUserDAL).UnLink(grpId, userId);
            return await getuser(new ReqData<int>(userId));
        });


        #endregion


        #region Group Api

        async public Task<IActionResult> getallgroups() => await safe(async () =>
            Data(await (dal as IGroupDAL).All()));

        [HttpPost]
        async public Task<IActionResult> creategroup([FromBody] ReqData<string> data) => await safe(async () =>
        {
            (dal as IGroupDAL).Create(data.Value);
            return await getallgroups();
        });


        [HttpPost]
        async public Task<IActionResult> deletegroup([FromBody] ReqData<int> data) => await safe(async () =>
        {
            (dal as IGroupDAL).Delete(data.Value);
            return await getallgroups();
        });


        [HttpPost]
        async public Task<IActionResult> getgroup([FromBody] ReqData<int> data) => await safe(async () =>
        {
            int grpId = data.Value;
            var grp = await (dal as IGroupDAL).Find(grpId);
            var subgroups = await (dal as IGroupDAL).FindAsParent(grpId);
            return Data(new { grp, subgroups });
        });


        [HttpPost]
        async public Task<IActionResult> linkgroupgroup([FromBody] ReqData<KeyValuePair<int, string>> data) => await safe(async () =>
        {
            var (parentId, grpName) = (data.Value.Key, data.Value.Value);
            var grp = await (dal as IGroupDAL).FindByName(grpName);
            if (grp == null) throw new Exception($"Group `{grpName}` not found");
            (dal as IGroupDAL).Link(parentId, grp.Id);
            return await getgroup(new ReqData<int>(parentId));
        });


        [HttpPost]
        async public Task<IActionResult> unlinkgroupgroup([FromBody] ReqData<KeyValuePair<int, int>> data) => await safe(async () =>
        {
            var (grpId, childId) = (data.Value.Key, data.Value.Value);
            (dal as IGroupDAL).UnLink(grpId, childId);
            return await getgroup(new ReqData<int>(grpId));
        });

        #endregion


        // -------------------------------------------------------------------------------
        private IActionResult Data(object data) => Json(new { data, toast = new { text = "", color = "", on = false } });
        private IActionResult OK(string msg) => Json(new { toast = new { text = msg, color = "green", on = true } });
        private IActionResult Fail(string msg) => Json(new { toast = new { text = msg, color = "red", on = true } });
        private IActionResult safe(Func<IActionResult> act) { try { return act(); } catch (Exception ex) { return Fail(ex.Message 
            //+ " " + ex.StackTrace
            ); } }
        async Task<IActionResult> safe(Func<Task<IActionResult>> act) { try { return await act(); } catch (Exception ex) { return Fail(ex.Message 
            //+ " " + ex.StackTrace
            ); } }
    }
}
