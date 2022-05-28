using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class DBClass
    {
        private string PATH = ExcelClass.bingPathToAppDir(@"DataBase\db.xlsx");
        private const int SHEET = 1;


        private const int USERNAME_COLUMN = 1;
        private const int PASSWORD_COLUMN = 2;
        private const int ID_COLUMN       = 3;
        private ExcelClass db;//      
        private int endOfDB;
        static private DBClass dbInstance = null;


        private DBClass()
        {
            db = new ExcelClass(PATH, 1, false);
            endOfDB = db.getLastRow();
        }

        public static DBClass getInstance()
        {
            if(dbInstance == null)
            {
                dbInstance = new DBClass();
            }

            return dbInstance;
        }

        public string getFromDB(int x,int y) { return db.readCell(x, y); }

        internal  bool drIsInDB(Doctor dr)
        {
            //check all user names
            for(int i = 1; i <= endOfDB; i++)
            {
                if (db.readCell(i, USERNAME_COLUMN) == dr.Username)
                    return true;
            }

            //looped through all the users so it means its not in the db
            return false;
        }
        internal  bool usernameIsInDB(string usr)
        {
            //check all user names
            for (int i = 1; i <= endOfDB; i++)
            {
                if (db.readCell(i, USERNAME_COLUMN) == usr)
                    return true;
            }

            //looped through all the users so it means its not in the db
            return false;
        }

        internal  bool idIsInDB(string usr)
        {
            //check all user names
            for (int i = 1; i <= endOfDB; i++)
            {
                if (db.readCell(i, ID_COLUMN) == usr)
                    return true;
            }

            //looped through all the users so it means its not in the db
            return false;
        }

        internal  bool addDrToDB(Doctor dr)
        {
            //check for null
            if (dr == null)
            {
                MessageBox.Show("dr == null");
                //return;
                return false;
            }

            //endOfDB+1 because we didnt save it
            if (!writeDrToDB(dr))
            {
                MessageBox.Show("failed to save doctor");
                //return;
                return false;
            }

            //end
            //return;
            return true;
        }

        public  bool writeDrToDB(Doctor dr)
        {
            db.openWb(PATH, SHEET);
            db.writeToCell(endOfDB + 1, USERNAME_COLUMN, dr.Username);
            db.writeToCell(endOfDB + 1, PASSWORD_COLUMN, dr.Password);
            db.writeToCell(endOfDB + 1, ID_COLUMN      , dr.Id);
            db.save();
            endOfDB++;
            

            return true;
        }

        public void setToDel()
        {
            db.Exit();

            dbInstance = null;
        }
        
        //Login methods
        public int getUsernameIndex(string usr)
        {
            //check all user names
            for (int i = 1; i <= endOfDB; i++)
            {
                if (db.readCell(i, USERNAME_COLUMN) == usr)
                    return i;
            }

            //looped through all the users so it means its not in the db
            return -1;
        }

        public Doctor loginToDb(string usr,string pass)
        {
            /**
             *  this method check if:
             *  1)  a user exists in db
             *  2)  compares the pass to the pass in the db
             *  3)  return the doctor object
             *      
             */

            int indx = getUsernameIndex(usr);
            if (indx == -1) { 
                //maybe add a message that username doesnt exists
                return null;
            }

            //check password
            if (db.readCell(indx, PASSWORD_COLUMN) != pass)
            {
                //maybe add a message that username doesnt exists
                return null;
            }

            //maybe add a message that username doesnt exists
            Doctor dr = new Doctor(getFromDB(indx, USERNAME_COLUMN),
                                   getFromDB(indx, PASSWORD_COLUMN),
                                   getFromDB(indx, ID_COLUMN));
            return dr;
        }



        public void Finish()
        {
            if (db != null)
            {
                db.Exit();
                db.Finish();
                db = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

    }
}
