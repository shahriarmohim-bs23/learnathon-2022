namespace backend_task3.Models
{
    public class Pagination
    {

       public int totalpage;
       public  int pagesize;
       public int currentpage;
      public  int totalitem;
     
        public  Pagination(int pagesize, int currentpage, int totalitem)
        {
            this.pagesize = pagesize;
            this.currentpage = currentpage;
            this.totalitem = totalitem;
            totalpage = (int)Math.Ceiling(totalitem / (double)pagesize); 
               
        }

       
    }
}
