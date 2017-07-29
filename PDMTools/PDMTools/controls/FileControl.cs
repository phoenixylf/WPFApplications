﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;
using PDMTools.datas;
using PDMTools.defined;

namespace PDMTools.controls
{
    public class FileControl
    {
        private MainWindow mWin;

        public FileControl(MainWindow win)
        {
            mWin = win;
        }

        public Operate calcFileVersion(Operate op)
        {
            if (null == op || Defined.OperateType.CalcFileVersion != op.type)
            {
                return null;
            }
            
            string filename = System.IO.Path.GetFileNameWithoutExtension(op.value);
            Regex rgx = new Regex(@"[\d]{1,4}[.][\d]{1,4}[.][\d]{1,4}[.][\d]{1,4}");
            string version = rgx.Match(filename).ToString();
            if (null == version)
            {
                return null;
            }

            Operate newOp = new Operate();
            newOp.type = Defined.OperateType.ReplaceWord;
            newOp.key = op.key;
            newOp.value = version;
            return newOp;
        }

        public Operate calcFileMd5(Operate op)
        {
            if (null == op || Defined.OperateType.CalcFileMd5 != op.type)
            {
                return null;
            }

            String md5 = null;
            try
            {
                FileStream file = new FileStream(op.value, FileMode.Open);
                MD5 md5Provider = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5Provider.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }

                md5 = sb.ToString() ;
            }
            catch (Exception)
            {
                return null;
            }

            Operate newOp = new Operate();
            newOp.type = Defined.OperateType.ReplaceWord;
            newOp.key = op.key;
            newOp.value = md5.ToUpper();
            return newOp;
        }

        public Operate calcFileModifiedTime(Operate op)
        {
            if (null == op || Defined.OperateType.CalcFileModifiedTime != op.type)
            {
                return null;
            }

            FileInfo fi = null;
            try
            {
                fi = new FileInfo(op.value);
            }
            catch (Exception)
            {
                return null;
            }

            if (null == fi)
            {
                return null;
            }

            Operate newOp = new Operate();
            newOp.type = Defined.OperateType.ReplaceWord;
            newOp.key = op.key;
            newOp.value = string.Format("{0:yyyy-MM-dd HH:mm:ss}", fi.LastWriteTime);
            return newOp;
        }

        public Operate calcFileSizeBytes(Operate op)
        {
            if (null == op || Defined.OperateType.CalcFileSizeByBytes != op.type)
            {
                return null;
            }

            FileInfo fi = null;
            try
            {
                fi = new FileInfo(op.value);
            }
            catch (Exception)
            {
                return null;
            }

            if (null == fi)
            {
                return null;
            }

            Operate newOp = new Operate();
            newOp.type = Defined.OperateType.ReplaceWord;
            newOp.key = op.key;
            newOp.value = string.Format("{0:N0} {1}", 
                fi.Length, mWin.FindResource("byte_unit"));
            return newOp;
        }

        public Operate calcFileSizeByM(Operate op)
        {
            if (null == op || Defined.OperateType.CalcFileSizeByM != op.type)
            {
                return null;
            }

            FileInfo fi = null;
            try
            {
                fi = new FileInfo(op.value);
            }
            catch (Exception)
            {
                return null;
            }

            if (null == fi)
            {
                return null;
            }

            Operate newOp = new Operate();
            newOp.type = Defined.OperateType.ReplaceWord;
            newOp.key = op.key;
            newOp.value = string.Format("{0:N1} {1}",
                fi.Length/1024/1024, mWin.FindResource("million_bytes_unit"));
            return newOp;
        }
    }
}
