using System;
using System.Collections.Generic;
using System.Text;
using Firefly.Box;
using ENV;
using ENV.IO;
using ENV.Data;

namespace $rootnamespace$
{
    public class $safeitemname$:BusinessProcessBase
    {
        //TODO: add the following code the the ApplicationPrograms Constructor:
        //Add("$safeitemname$", typeof ($rootnamespace$.$safeitemname$));

    
        ENV.IO.TextTemplate page = new ENV.IO.TextTemplate("$safeitemname$.html"); 
        ENV.IO.WebWriter web = new ENV.IO.WebWriter();
        
        public $safeitemname$()
        {

            Streams.Add(web);
        }
        
        protected override void OnLeaveRow()
        {
            page.WriteTo(web);
        }      
        
        public void Run()
        {
            Execute();
        }
  
    }
}