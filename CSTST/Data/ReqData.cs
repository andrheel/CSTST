namespace CSTST.Controllers
{
    public class ReqData<T>
    {
        public ReqData(T _Value) {  Value = _Value; }

        public T Value { get; set; }
    }

}