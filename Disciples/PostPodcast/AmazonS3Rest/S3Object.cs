// This software code is made available "AS IS" without warranties of any        
// kind.  You may copy, display, modify and redistribute the software            
// code either by itself or as incorporated into your code; provided that        
// you do not remove any proprietary notices.  Your use of this software         
// code is at your own risk and you waive any claim against Amazon               
// Digital Services, Inc. or its affiliates with respect to your use of          
// this software code. (c) 2006 Amazon Digital Services, Inc. or its             
// affiliates.          



using System;
using System.Collections;
using System.Text;
using System.IO;


namespace AmazonS3REST
{
    public class S3Object
    {
        private SortedList metadata;
        private string data;

        public string Data {
            get {
                return data;
            }
        }

       public SortedList Metadata {
            get {
                return metadata;
            }
        }


        public S3Object(string data, SortedList metadata)
        {

            this.data = data;
            this.metadata = metadata;
        }

    }
}
