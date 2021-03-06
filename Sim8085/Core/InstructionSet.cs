﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sim8085.Core
{
    public static class InstructionSet
    {
        public static void MOV(ref string line,LineAssembleResult result)
        {
            result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"MOV\s+(\w)\s*\,\s*(\w)");
            if(match.Success)
            {
                Register to = AssemblyUtility.IsRegister(match.Groups[1].Value);
                Register from = AssemblyUtility.IsRegister(match.Groups[2].Value);

                if (to!=null && from != null)
                {
                    if (match.Groups[1].Value == "M" && match.Groups[2].Value == "M")
                    {
                        I8085.M = I8085.M;
                    }
                    else if (match.Groups[1].Value == "M")
                    {
                        I8085.Memory[I8085.M].Data = from.Value;
                        result.RegistersChanged.Add(new Register(I8085.M.ToString().PadLeft(4,'0'),
                            null, I8085.Memory[I8085.M].Data));
                    }
                    else if (match.Groups[2].Value == "M")
                    {
                        to.Value = I8085.Memory[I8085.M].Data;
                        result.RegistersChanged.Add(to);
                    }
                    else
                    {
                        to.Value = from.Value;
                        result.RegistersChanged.Add(to);
                    }

                }
                else
                {
                    result.SetError("MOV instruction work with Register to Register Only.");
                    return;
                }

                line = line.Replace(match.Groups[0].Value,string.Empty);

            }
            else
            {
                result.SetError("MOV instruction is incorrectly formatted");
                return;
            }
        }
        public static void MVI(ref string line,LineAssembleResult result)
        {
            result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"MVI\s+([a-zA-Z])\s*\,\s*(\d{1,}H?)");
            if(match.Success)
            {
                Register to = AssemblyUtility.IsRegister(match.Groups[1].Value);
                if(to!=null)
                {
                    bool valueHexType = match.Groups[2].Value.ToLower().IndexOf("h") > 0;
                    byte value;
                    if (valueHexType)
                    {
                        string val = match.Groups[2].Value.ToLower().Replace("h", string.Empty);
                        value = Convert.ToByte(val, 16);
                    }
                    else
                    {
                        value= Convert.ToByte(match.Groups[2].Value, 10);
                    }

                    if(value>=0&&value<=255)
                    {
                        if (match.Groups[1].Value == "M")
                        {
                            I8085.Memory[I8085.M].Data = value;
                            result.RegistersChanged.Add(new Register(I8085.M.ToString().PadLeft(4, '0'),
                            null, I8085.Memory[I8085.M].Data));
                        }
                        else
                        {
                            to.Value = value;
                            result.RegistersChanged.Add(to);
                        }
                    }
                    else
                    {
                        result.SetError("MVI should utilized 8 bit data only ie from 00-FF or 0-255");
                        return;
                    }
                    line = line.Replace(match.Groups[0].Value, string.Empty);
                }
                else
                {
                    result.SetError("MVI instruction work with Register to Register Only.");
                    return;
                }
            }
            else
            {
                result.SetError("MVI instruction is incorrectly formatted"+line);
                return;
            }
        }
        public static void LDA(ref string line, LineAssembleResult result)
        {
            result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"LDA\s+(\d{1,}H?)");
            if (match.Success)
            {
                bool valueHexType = match.Groups[1].Value.ToLower().IndexOf("h") > 0;
                int value;
                if (valueHexType)
                {
                    string val = match.Groups[1].Value.ToLower().Replace("h", string.Empty);
                    value = Convert.ToInt32(val, 16);
                }
                else
                {
                    value = Convert.ToInt32(match.Groups[1].Value, 10);
                }

                if (value >= 0 && value <= 65535)
                {
                    I8085.A.Value = I8085.Memory[value].Data;
                    result.RegistersChanged.Add(I8085.A);
                }
                else
                {
                    result.SetError("LDA should utilized 16 bit data only ie from 00-65535");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("LDA instruction is incorrectly formatted" + line);
                return;
            }
        }
        public static void STA(ref string line, LineAssembleResult result)
        {
            result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"STA\s+(\d{1,}H?)");
            if (match.Success)
            {
                bool valueHexType = match.Groups[1].Value.ToLower().IndexOf("h") > 0;
                int value;
                if (valueHexType)
                {
                    string val = match.Groups[1].Value.ToLower().Replace("h", string.Empty);
                    value = Convert.ToInt32(val, 16);
                }
                else
                {
                    value = Convert.ToInt32(match.Groups[1].Value, 10);
                }

                if (value >= 0 && value <= 65535)
                {
                    I8085.Memory[value].Data= I8085.A.Value;
                    result.RegistersChanged.Add(new Register(value.ToString().PadLeft(4,'0')
                        ,null,I8085.Memory[value].Data));
                }
                else
                {
                    result.SetError("STA should utilized 16 bit data only ie from 00-65535");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("STA instruction is incorrectly formatted" + line);
                return;
            }
        }


        public static void ADD(ref string line, LineAssembleResult result)
        {
            result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"ADD\s+([\w])");
            if (match.Success)
            {
                Register from = AssemblyUtility.IsRegister(match.Groups[1].Value);
                if (from != null)
                {
                    if (match.Groups[1].Value == "M")
                    {
                        AssemblyUtility.AddAdjustFlags(I8085.A.Value, I8085.Memory[I8085.M].Data);
                        I8085.A.Value+=I8085.Memory[I8085.M].Data;
                        result.RegistersChanged.Add(I8085.A);
                    }
                    else
                    {
                        AssemblyUtility.AddAdjustFlags(I8085.A.Value, from.Value);
                        I8085.A.Value += from.Value;
                        result.RegistersChanged.Add(I8085.A);
                    }

                }
                else
                {
                    result.SetError("ADD instruction work with  Register Only.");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("ADD instruction is incorrectly formatted");
                return;
            }
        }
        public static void ADI(ref string line, LineAssembleResult result)
        {
            result.FutureLineNumber = result.LineNumber + 1;


            var match = Regex.Match(line, @"ADI\s+(\d{1,}H?)");
            if (match.Success)
            {
                bool valueHexType = match.Groups[1].Value.ToLower().IndexOf("h") > 0;
                byte value;
                if (valueHexType)
                {
                    string val = match.Groups[1].Value.ToLower().Replace("h", string.Empty);
                    value = Convert.ToByte(val, 16);
                }
                else
                {
                    value = Convert.ToByte(match.Groups[1].Value, 10);
                }
                if (value >= 0 && value <= 255)
                {
                    AssemblyUtility.AddAdjustFlags(I8085.A.Value, value);
                    I8085.A.Value+= value;
                    result.RegistersChanged.Add(I8085.A);
                }
                else
                {
                    result.SetError("ADI should utilized 8 bit data only ie from 00-FF or 0-255");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("ADI instruction is incorrectly formatted");
                return;
            }
        }
        public static void ADC(ref string line, LineAssembleResult result)
        {
            result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"ADC\s+([\w])");
            if (match.Success)
            {
                Register from = AssemblyUtility.IsRegister(match.Groups[1].Value);
                if (from != null)
                {
                    if (match.Groups[1].Value == "M")
                    {
                        AssemblyUtility.AddAdjustFlags(I8085.A.Value, I8085.Memory[I8085.M].Data,
                            Convert.ToByte(I8085.Flag_C.Value));

                        I8085.A.Value += I8085.Memory[I8085.M].Data;
                        I8085.A.Value += Convert.ToByte(I8085.Flag_C.Value);

                        result.RegistersChanged.Add(I8085.A);
                    }
                    else
                    {
                        AssemblyUtility.AddAdjustFlags(I8085.A.Value, from.Value, Convert.ToByte(I8085.Flag_C.Value));
                        I8085.A.Value += from.Value;
                        I8085.A.Value += Convert.ToByte(I8085.Flag_C.Value);
                        result.RegistersChanged.Add(I8085.A);
                    }


                }
                else
                {
                    result.SetError("ADC instruction work with Register Only.");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("ADC instruction is incorrectly formatted");
                return;
            }
        }
        public static void ACI(ref string line, LineAssembleResult result)
        {
            result.FutureLineNumber = result.LineNumber + 1;


            var match = Regex.Match(line, @"ACI\s+(\d{1,}H?)");
            if (match.Success)
            {
                bool valueHexType = match.Groups[1].Value.ToLower().IndexOf("h") > 0;
                byte value;
                if (valueHexType)
                {
                    string val = match.Groups[1].Value.ToLower().Replace("h", string.Empty);
                    value = Convert.ToByte(val, 16);
                }
                else
                {
                    value = Convert.ToByte(match.Groups[1].Value, 10);
                }
                if (value >= 0 && value <= 255)
                {
                    AssemblyUtility.AddAdjustFlags(I8085.A.Value, value, Convert.ToByte(I8085.Flag_C.Value));
                    I8085.A.Value += value;
                    result.RegistersChanged.Add(I8085.A);
                }
                else
                {
                    result.SetError("ACI should utilized 8 bit data only ie from 00-FF or 0-255");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("ACI instruction is incorrectly formatted");
                return;
            }
        }

        public static void SUB(ref string line, LineAssembleResult result)
        {
            result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"SUB\s+([\w])");
            if (match.Success)
            {
                Register from = AssemblyUtility.IsRegister(match.Groups[1].Value);
                if (from != null)
                {
                    if (match.Groups[1].Value == "M")
                    {
                        AssemblyUtility.SubAdjustFlags(I8085.A.Value, I8085.Memory[I8085.M].Data);

                        I8085.A.Value -= I8085.Memory[I8085.M].Data;

                        result.RegistersChanged.Add(I8085.A);
                    }
                    else
                    {
                        AssemblyUtility.SubAdjustFlags(I8085.A.Value, from.Value);
                        I8085.A.Value -= from.Value;
                        result.RegistersChanged.Add(I8085.A);
                    }

                }
                else
                {
                    result.SetError("SUB instruction work with  Register Only.");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("SUB instruction is incorrectly formatted");
                return;
            }
        }
        public static void SUI(ref string line, LineAssembleResult result)
        {
            result.FutureLineNumber = result.LineNumber + 1;


            var match = Regex.Match(line, @"SUI\s+(\d{1,}H?)");
            if (match.Success)
            {
                bool valueHexType = match.Groups[1].Value.ToLower().IndexOf("h") > 0;
                byte value;
                if (valueHexType)
                {
                    string val = match.Groups[1].Value.ToLower().Replace("h", string.Empty);
                    value = Convert.ToByte(val, 16);
                }
                else
                {
                    value = Convert.ToByte(match.Groups[1].Value, 10);
                }
                if (value >= 0 && value <= 255)
                {
                    AssemblyUtility.SubAdjustFlags(I8085.A.Value, value);
                    I8085.A.Value -= value;
                    result.RegistersChanged.Add(I8085.A);
                }
                else
                {
                    result.SetError("SUI should utilized 8 bit data only ie from 00-FF or 0-255");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("SUI instruction is incorrectly formatted");
                return;
            }
        }
        public static void SBB(ref string line, LineAssembleResult result)
        {
            result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"SBB\s+([\w])");
            if (match.Success)
            {
                Register from = AssemblyUtility.IsRegister(match.Groups[1].Value);
                if (from != null)
                {
                    if (match.Groups[1].Value == "M")
                    {
                        AssemblyUtility.SubAdjustFlags(I8085.A.Value, I8085.Memory[I8085.M].Data,
                            Convert.ToByte(I8085.Flag_C.Value));

                        I8085.A.Value -= I8085.Memory[I8085.M].Data;
                        I8085.A.Value -= Convert.ToByte(I8085.Flag_C.Value);

                        result.RegistersChanged.Add(I8085.A);
                    }
                    else
                    {
                        AssemblyUtility.SubAdjustFlags(I8085.A.Value, from.Value, Convert.ToByte(I8085.Flag_C.Value));
                        I8085.A.Value -= from.Value;
                        I8085.A.Value -= (byte)(I8085.Flag_C.Value ? 1 : 0);
                        result.RegistersChanged.Add(I8085.A);
                    }

                }
                else
                {
                    result.SetError("SBB instruction work with  Register Only.");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("SBB instruction is incorrectly formatted");
                return;
            }
        }
        public static void SBI(ref string line, LineAssembleResult result)
        {
            result.FutureLineNumber = result.LineNumber + 1;


            var match = Regex.Match(line, @"SBI\s+(\d{1,}H?)");
            if (match.Success)
            {
                bool valueHexType = match.Groups[1].Value.ToLower().IndexOf("h") > 0;
                byte value;
                if (valueHexType)
                {
                    string val = match.Groups[1].Value.ToLower().Replace("h", string.Empty);
                    value = Convert.ToByte(val, 16);
                }
                else
                {
                    value = Convert.ToByte(match.Groups[1].Value, 10);
                }
                if (value >= 0 && value <= 255)
                {
                    AssemblyUtility.SubAdjustFlags(I8085.A.Value, value, Convert.ToByte(I8085.Flag_C.Value));
                    I8085.A.Value -= value;
                    I8085.A.Value -= (byte)(I8085.Flag_C.Value ? 1 : 0);
                    result.RegistersChanged.Add(I8085.A);
                }
                else
                {
                    result.SetError("SBI should utilized 8 bit data only ie from 00-FF or 0-255");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("SBI instruction is incorrectly formatted");
                return;
            }
        }


        public static void HLT(ref string line, LineAssembleResult result)
        {

            var match = Regex.Match(line, @"HLT");
            if (match.Success)
            {
                result.IsHalt = true;
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("HLT instruction is incorrectly formatted");
                return;
            }
        }
        public static void CMA(ref string line, LineAssembleResult result)
        {
            result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"CMA");
            if (match.Success)
            {
                I8085.A.Value=AssemblyUtility.Compliment(I8085.A.Value);
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("CMA instruction is incorrectly formatted");
                return;
            }
        }


        public static void JNZ(ref string line, LineAssembleResult result)
        {
            //result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"JNZ\s+(\w*)");
            if (match.Success)
            {
                string label=match.Groups[1].Value;
                
                
                if (Assembler.LabelsCollection.ContainsKey(label))
                {
                    if (I8085.Flag_Z.Value == false)
                        result.FutureLineNumber = Assembler.LabelsCollection[label];
                    else
                        result.FutureLineNumber = result.LineNumber + 1;
                }
                else
                {
                    result.SetError($"JNZ label: {label} not found");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("JNZ instruction is incorrectly formatted" + line);
                return;
            }
        }
        public static void JZ(ref string line, LineAssembleResult result)
        {
            //result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"JZ\s+(\w*)");
            if (match.Success)
            {
                string label = match.Groups[1].Value;


                if (Assembler.LabelsCollection.ContainsKey(label))
                {
                    if (I8085.Flag_Z.Value)
                        result.FutureLineNumber = Assembler.LabelsCollection[label];
                    else
                        result.FutureLineNumber = result.LineNumber + 1;
                }
                else
                {
                    result.SetError($"JZ label: {label} not found");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("JZ instruction is incorrectly formatted" + line);
                return;
            }
        }
        public static void JNC(ref string line, LineAssembleResult result)
        {
            //result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"JNC\s+(\w*)");
            if (match.Success)
            {
                string label = match.Groups[1].Value;


                if (Assembler.LabelsCollection.ContainsKey(label))
                {
                    if (I8085.Flag_C.Value == false)
                        result.FutureLineNumber = Assembler.LabelsCollection[label];
                    else
                        result.FutureLineNumber = result.LineNumber + 1;
                }
                else
                {
                    result.SetError($"JNC label: {label} not found");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("JNC instruction is incorrectly formatted" + line);
                return;
            }
        }
        public static void JC(ref string line, LineAssembleResult result)
        {
            //result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"JC\s+(\w*)");
            if (match.Success)
            {
                string label = match.Groups[1].Value;


                if (Assembler.LabelsCollection.ContainsKey(label))
                {
                    if (I8085.Flag_C.Value)
                        result.FutureLineNumber = Assembler.LabelsCollection[label];
                    else
                        result.FutureLineNumber = result.LineNumber + 1;
                }
                else
                {
                    result.SetError($"JC label: {label} not found");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("JC instruction is incorrectly formatted" + line);
                return;
            }
        }
        public static void JNP(ref string line, LineAssembleResult result)
        {
            //result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"JNP\s+(\w*)");
            if (match.Success)
            {
                string label = match.Groups[1].Value;


                if (Assembler.LabelsCollection.ContainsKey(label))
                {
                    if (I8085.Flag_S.Value == false)
                        result.FutureLineNumber = Assembler.LabelsCollection[label];
                    else
                        result.FutureLineNumber = result.LineNumber + 1;
                }
                else
                {
                    result.SetError($"JNP label: {label} not found");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("JNP instruction is incorrectly formatted" + line);
                return;
            }
        }
        public static void JP(ref string line, LineAssembleResult result)
        {
            //result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"JP\s+(\w*)");
            if (match.Success)
            {
                string label = match.Groups[1].Value;


                if (Assembler.LabelsCollection.ContainsKey(label))
                {
                    if (I8085.Flag_S.Value)
                        result.FutureLineNumber = Assembler.LabelsCollection[label];
                    else
                        result.FutureLineNumber = result.LineNumber + 1;
                }
                else
                {
                    result.SetError($"JP label: {label} not found");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("JP instruction is incorrectly formatted" + line);
                return;
            }
        }


        public static void INR(ref string line, LineAssembleResult result)
        {
            result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"INR\s+([\w])");
            if (match.Success)
            {
                Register from = AssemblyUtility.IsRegister(match.Groups[1].Value);
                if (from != null)
                {
                    if(match.Groups[1].Value=="M")
                    {
                        AssemblyUtility.AddAdjustFlags(I8085.M, 1,false);
                        I8085.M += 1;
                        if (I8085.M < I8085.MemorySize && I8085.M >= 0)
                        {
                            result.RegistersChanged.Add(new Register(I8085.M.ToString().PadLeft(4, '0'),
                                                       null, I8085.Memory[I8085.M].Data));
                        }
                        else
                        {
                            result.SetError("INR instruction value out of bound:"+I8085.M);
                            return;
                        }
                    }
                    else
                    {
                        AssemblyUtility.AddAdjustFlags(from.Value,1);
                        from.Value += 1;
                        result.RegistersChanged.Add(from);
                    }

                }
                else
                {
                    result.SetError("INR instruction work with  Register Only.");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("INR instruction is incorrectly formatted");
                return;
            }
        }
        public static void DCR(ref string line, LineAssembleResult result)
        {
            result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"DCR\s+([\w])");
            if (match.Success)
            {
                Register from = AssemblyUtility.IsRegister(match.Groups[1].Value);
                if (from != null)
                {
                    if (match.Groups[1].Value == "M")
                    {
                        AssemblyUtility.AddAdjustFlags(I8085.M, 1, false);
                        I8085.M -= 1;
                        if (I8085.M < I8085.MemorySize && I8085.M >= 0)
                        {
                            result.RegistersChanged.Add(new Register(I8085.M.ToString().PadLeft(4, '0'),
                                                   null, I8085.Memory[I8085.M].Data));
                        }
                        else
                        {
                            result.SetError("INR instruction value out of bound:" + I8085.M);
                            return;
                        }
                        
                    }
                    else
                    {
                        AssemblyUtility.SubAdjustFlags(from.Value, 1,false);
                        from.Value -= 1;
                        result.RegistersChanged.Add(I8085.A);
                    }

                }
                else
                {
                    result.SetError("DCR instruction work with  Register Only.");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("DCR instruction is incorrectly formatted");
                return;
            }
        }

        public static void CMP(ref string line, LineAssembleResult result)
        {
            result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"CMP\s+([\w])");
            if (match.Success)
            {
                Register from = AssemblyUtility.IsRegister(match.Groups[1].Value);
                if (from != null)
                {
                    byte value1;
                    if (match.Groups[1].Value == "M")
                    {
                        if (I8085.M < I8085.MemorySize && I8085.M >= 0)
                        {
                            value1 = I8085.Memory[I8085.M].Data;
                        }
                        else
                        {
                            result.SetError("CMP instruction value out of bound:" + I8085.M);
                            return;
                        }

                    }
                    else
                    {
                        value1 = from.Value;
                    }

                    if(I8085.A.Value<value1)
                    {
                        I8085.Flag_C.Value = true;
                        I8085.Flag_Z.Value = false;

                    }
                    else if(I8085.A.Value==value1)
                    {
                        I8085.Flag_C.Value = false;
                        I8085.Flag_Z.Value = true;
                    }
                    else
                    {
                        I8085.Flag_C.Value = false;
                        I8085.Flag_Z.Value = false;
                    }

                }
                else
                {
                    result.SetError("CMP instruction work with  Register Only.");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("CMP instruction is incorrectly formatted");
                return;
            }
        }
        public static void CPI(ref string line, LineAssembleResult result)
        {
            result.FutureLineNumber = result.LineNumber + 1;


            var match = Regex.Match(line, @"CPI\s+(\d{1,}H?)");
            if (match.Success)
            {
                bool valueHexType = match.Groups[1].Value.ToLower().IndexOf("h") > 0;
                byte value;
                if (valueHexType)
                {
                    string val = match.Groups[1].Value.ToLower().Replace("h", string.Empty);
                    value = Convert.ToByte(val, 16);
                }
                else
                {
                    value = Convert.ToByte(match.Groups[1].Value, 10);
                }
                if (value >= 0 && value <= 255)
                {
                    if (I8085.A.Value < value)
                    {
                        I8085.Flag_C.Value = true;
                        I8085.Flag_Z.Value = false;

                    }
                    else if (I8085.A.Value == value)
                    {
                        I8085.Flag_C.Value = false;
                        I8085.Flag_Z.Value = true;
                    }
                    else
                    {
                        I8085.Flag_C.Value = false;
                        I8085.Flag_Z.Value = false;
                    }
                }
                else
                {
                    result.SetError("CPI should utilized 8 bit data only ie from 00-FF or 0-255");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("CPI instruction is incorrectly formatted");
                return;
            }
        }

        public static void CALL(ref string line, LineAssembleResult result)
        {
            //result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"CALL\s+(\w*)");
            if (match.Success)
            {
                string label = match.Groups[1].Value;


                if (Assembler.LabelsCollection.ContainsKey(label))
                {
                   result.FutureLineNumber = Assembler.LabelsCollection[label];
                   Assembler.CallStack.Add(result.LineNumber);
                }
                else
                {
                    result.SetError($"CALL label: {label} not found");
                    return;
                }
                line = line.Replace(match.Groups[0].Value, string.Empty);
            }
            else
            {
                result.SetError("CALL instruction is incorrectly formatted" + line);
                return;
            }
        }
        public static void RET(ref string line, LineAssembleResult result)
        {
            result.FutureLineNumber = result.LineNumber + 1;

            var match = Regex.Match(line, @"RET");
            if (match.Success)
            {
                if(Assembler.CallStack.Count>0)
                {
                    result.FutureLineNumber = Assembler.CallStack.Last()+1;
                    Assembler.CallStack.RemoveAt(Assembler.CallStack.Count - 1);
                }
                else
                {
                    result.SetError("RET can't be executed since no value in callstack to go.");
                    return;
                }
            }
            else
            {
                result.SetError("RET instruction is incorrectly formatted");
                return;
            }
        }




    }
}
