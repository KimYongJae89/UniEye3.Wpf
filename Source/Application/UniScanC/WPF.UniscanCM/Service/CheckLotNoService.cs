using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanC.Data;
using WPF.UniScanCM.Override;

namespace WPF.UniScanCM.Service
{
    public static class CheckLotNoService
    {
        private static string ReadFilePath = BaseConfig.Instance().ConfigPath + @"\LotNumber.txt";
        private static string WriteFilePath = BaseConfig.Instance().ConfigPath + @"\LotNumber.txt";

        public static string CheckLotNo(InspectDataImporter dataImporter, string lotNo)
        {
            string newLotNo = lotNo;

            // 자동 LotNo 부여 기능
            if (SystemConfig.Instance.UseAutoLotNo)
            {
                // 고객사별 원하는 Lot 형식
                switch (SystemConfig.Instance.Customer)
                {
                    case Enums.ECustomer.Samsung: newLotNo = MakeSamsungLotNo(lotNo); break;
                    default: break;
                }
            }

            // 비어있는 Lot 방지
            if (string.IsNullOrEmpty(newLotNo))
            {
                newLotNo = "NewLot";
            }

            //중복된 Lot가 없을 경우 그대로 진행
            if (!dataImporter.ImportIsExistLotName(newLotNo))
            {
                return newLotNo;
            }

            List<Dictionary<string, object>> lotNames = dataImporter.ImportSameLotNames(lotNo);
            //중복된 Lot가 있고 처음으로 중복된 경우라면 @1 추가
            if (lotNames.Count == 0)
            {
                newLotNo = $"{newLotNo}@1";
            }
            //이미 @1번 이후 중복이 있을 경우
            else
            {
                int maxIndex = -1;
                foreach (Dictionary<string, object> lotNamePair in lotNames)
                {
                    string lotName = lotNamePair["lot_name"].ToString();
                    string[] lotSplit = lotName.Split('@');
                    if (lotSplit.Length == 2)
                    {
                        maxIndex = Math.Max(maxIndex, Convert.ToInt32(lotSplit[1]));
                    }
                }
                newLotNo = $"{newLotNo}@{maxIndex + 1}";
            }

            return newLotNo;
        }

        private static string MakeSamsungLotNo(string baseLotNo)
        {
            string newLotNo = baseLotNo;

            string deviceCode = SystemConfig.Instance.DeviceCode;
            string workplaceCode = SystemConfig.Instance.WorkplaceCode;

            var lotNameList = new List<string>();
            DateTime dateTime = DateTime.Now;
            int lotNumber;

            lotNumber = ReadLotNumber();
            WriteLotNumber(lotNumber);

            lotNameList.Add(deviceCode); // Machine Code

            int year = dateTime.Year;
            switch ((year - 2014) % 26)
            {
                case 0: lotNameList.Add("A"); break;
                case 1: lotNameList.Add("B"); break;
                case 2: lotNameList.Add("C"); break;
                case 3: lotNameList.Add("D"); break;
                case 4: lotNameList.Add("E"); break; // 2018
                case 5: lotNameList.Add("F"); break; // 2019
                case 6: lotNameList.Add("G"); break; // 2020
                case 7: lotNameList.Add("H"); break; // 2021
                case 8: lotNameList.Add("I"); break; // 2022
                case 9: lotNameList.Add("J"); break; // 2023
                case 10: lotNameList.Add("K"); break; // 2024
                case 11: lotNameList.Add("L"); break; // 2025
                case 12: lotNameList.Add("M"); break; // 2026
                case 13: lotNameList.Add("N"); break; // 2027
                case 14: lotNameList.Add("O"); break; // 2028
                case 15: lotNameList.Add("P"); break; // 2029
                case 16: lotNameList.Add("Q"); break; // 2030
                case 17: lotNameList.Add("R"); break; // 2031
                case 18: lotNameList.Add("S"); break; // 2032
                case 19: lotNameList.Add("T"); break; // 2033
                case 20: lotNameList.Add("U"); break; // 2034
                case 21: lotNameList.Add("V"); break; // 2035
                case 22: lotNameList.Add("W"); break; // 2036
                case 23: lotNameList.Add("X"); break; // 2037
                case 24: lotNameList.Add("Y"); break; // 2038
                case 25: lotNameList.Add("Z"); break; // 2039
                default: lotNameList.Add("0"); break;
            }

            lotNameList.Add(string.Format("{0:X}", dateTime.Month)); // Month Code
            lotNameList.Add(string.Format("{0:00}", dateTime.Day)); // Day Code
            lotNameList.Add(string.Format("{0:00}", lotNumber)); // Lot Number

            lotNameList.Add(string.Format(workplaceCode)); // Workplace
            lotNameList.Add(string.Format("A")); // Workplace

            char[] charLotName = baseLotNo.ToArray<char>();
            // Lot 가 비어있지 않을 경우
            if (!string.IsNullOrWhiteSpace(baseLotNo))
            {
                // 형식이 맞지 않다면 기존 방식 그대로
                if (charLotName.Length == 9)
                {
                    // Lot 형식을 맞게 했을 경우 현재 Lot의 번호를 가져오기 위한 코드
                    bool isSameDate = false;
                    // 장비 코드, 년도 비교
                    for (int i = 0; i < 2; i++)
                    {
                        if (lotNameList[i] == charLotName[i].ToString())
                        {
                            isSameDate = true;
                            continue;
                        }
                        else
                        {
                            isSameDate = false;
                            break;
                        }
                    }
                    // 월 비교
                    if (isSameDate == true && lotNameList[2] == charLotName[2].ToString())
                    {
                        isSameDate = true;
                    }
                    else
                    {
                        isSameDate = false;
                    }
                    // 일 비교
                    if (isSameDate == true && lotNameList[3] == charLotName[3].ToString() + charLotName[4].ToString())
                    {
                        isSameDate = true;
                    }
                    else
                    {
                        isSameDate = false;
                    }
                    // 형식이 맞다고 판단되면 지금 적힌 번호를 다시 시작점으로 설정하고 저장
                    if (isSameDate)
                    {
                        string rotNum = "";
                        for (int i = 5; i < 7; i++)
                        {
                            rotNum += charLotName[i];
                        }
                        WriteLotNumber(Convert.ToInt32(rotNum));
                    }
                }
            }
            // 비어있다면 Samsung 형식으로 만들어서 리턴
            else
            {
                for (int i = 0; i < lotNameList.Count; i++)
                {
                    newLotNo = newLotNo + lotNameList[i];
                }
            }
            return newLotNo;
        }

        private static void WriteLotNumber(int lotNumber)
        {
            var stringBuilder = new StringBuilder();

            string date = DateTime.Now.ToString("yyMMdd");

            stringBuilder.Append(string.Format("{0},{1}", date, lotNumber));
            File.WriteAllText(WriteFilePath, stringBuilder.ToString(), Encoding.UTF8);
        }

        private static int ReadLotNumber()
        {
            int lotNumber = 1;

            string[] result;
            string[] splitResult;

            DateTime dateTime = DateTime.Now;

            if (!File.Exists(ReadFilePath))
            {
                using (var file = new System.IO.StreamWriter(ReadFilePath, true))
                {
                    var stringBuilder = new StringBuilder();

                    string date = DateTime.Now.ToString("yyMMdd");

                    stringBuilder.Append(string.Format("{0},{1}", date, lotNumber));

                    file.Write(stringBuilder);
                    file.Flush();
                }
            }

            result = File.ReadAllLines(ReadFilePath);

            splitResult = result[0].Split(',');

            if (splitResult.Count() >= 2)
            {
                if (Convert.ToInt32(splitResult[0]) < Convert.ToInt32(dateTime.ToString("yyMMdd")))
                {
                    lotNumber = 1;
                }
                else
                {
                    lotNumber = Convert.ToInt32(splitResult[1]) + 1;
                }
            }

            return lotNumber;
        }
    }
}
