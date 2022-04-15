using System;
using System.IO;
using System.Text;
namespace Virtual_Machine
{
    class Program
    {
        static void Main(string[] args)
        {

            string commandFile;
            string encryptedFile;
            bool Program=true;
            string Output="Output.txt";
            while(Program)
            {
                Program=true;
                Console.WriteLine("1. Encrypt");
                Console.WriteLine("2. Decrypt");
                Console.WriteLine("3. Exit");
                int option=Convert.ToInt32(Console.ReadLine());

                switch(option)
                {
                    case 1:
                    {
                        byte[] regs=new byte[16];
                        byte[] prog_mem=new byte[256];
                        bool ieof_flag=false;
                        int fileIndex=0;
                        int programCounter=0;
                        bool RET=true;
                        bool flag=false;

                        Console.WriteLine("Write Command file (.bin)");
                        while(true)
                        {
                            commandFile=Console.ReadLine();
                            if(File.Exists(commandFile))
                            {
                                break;
                            }
                            else Console.WriteLine("Wrong file");
                        }
                        prog_mem=File.ReadAllBytes(commandFile);
                        Console.WriteLine("Write file to encrypt (.txt)");
                        while(true)
                        {
                            encryptedFile=Console.ReadLine();
                            if(File.Exists(encryptedFile))
                            {
                                encryptedFile=File.ReadAllText(encryptedFile);
                                break;
                            }
                            else Console.WriteLine("Wrong file");
                        }

                        Output="encryptedFile.txt";
                        System.IO.File.WriteAllText(Output,string.Empty);
                        Console.WriteLine($"Outputed into {Output}");
                        Console.WriteLine();
                        while(RET)
                        {
                            Commands(prog_mem[programCounter],prog_mem[programCounter+1],ref regs,encryptedFile,ref fileIndex,ref ieof_flag,ref programCounter,ref RET,ref flag,Output);
                            programCounter+=2;
                        }
                        Console.WriteLine();
                        Console.WriteLine();

                        break;
                    }
                    case 2:
                    {
                        byte[] regs=new byte[16];
                        byte[] prog_mem=new byte[256];
                        bool ieof_flag=false;
                        int fileIndex=0;
                        int programCounter=0;
                        bool RET=true;
                        bool flag=false;

                        Console.WriteLine("Write Command file (.bin)");
                        while(true)
                        {
                            commandFile=Console.ReadLine();
                            if(File.Exists(commandFile))
                            {
                                break;
                            }
                            else Console.WriteLine("Wrong file");
                        }
                        prog_mem=File.ReadAllBytes(commandFile);
                        Console.WriteLine("Write encrypted file (.txt)");
                        while(true)
                        {
                            encryptedFile=Console.ReadLine();
                            if(File.Exists(encryptedFile))
                            {
                                encryptedFile=File.ReadAllText(encryptedFile);
                                break;
                            }
                            else Console.WriteLine("Wrong file");
                        }

                        Output="decryptedFile.txt";
                        System.IO.File.WriteAllText(Output,string.Empty);
                        Console.WriteLine($"Outputed into {Output}");
                        Console.WriteLine();

                        while(RET)
                        {
                            Commands(prog_mem[programCounter],prog_mem[programCounter+1],ref regs,encryptedFile,ref fileIndex,ref ieof_flag,ref programCounter,ref RET,ref flag,Output);
                            programCounter+=2;
                        }
                        Console.WriteLine();
                        Console.WriteLine();

                        break;
                    }
                    case 3:
                    {
                        Program=false;
                        break;
                    }
                }
            }
        }

        static void Commands(byte code,byte param,ref byte[] regs,string encryptedFile,ref int fileIndex,ref bool ieof_flag,ref int programCounter,ref bool RET,ref bool flag,string fileName)
        {
            switch(code)
            {
                case 1:
                {
                    INC(param,ref regs,ref flag);
                    break;
                }
                case 2:
                {
                    DEC(param,ref regs,ref flag);
                    break;
                }
                case 3:
                {
                    MOV(param,ref regs);
                    break;
                }
                case 4:
                {
                    MOVC(param,ref regs);
                    break;
                }
                case 5:
                {
                    LSL(param,ref regs,ref flag);
                    break;
                }
                case 6:
                {
                    LSR(param,ref regs,ref flag);
                    break;
                }
                case 7:
                {
                    JMP(param,regs,ref programCounter);
                    break;
                }
                case 8:
                {
                    JZ(param,regs,flag,ref programCounter);
                    break;
                }
                case 9:
                {
                    JNZ(param,regs,flag,ref programCounter);
                    break;
                }
                case 10:
                {
                    JFE(param,regs,ieof_flag,ref programCounter);
                    break;
                }
                case 11:
                {
                    RET=false;
                    break;
                }
                case 12:
                {
                    ADD(param,ref regs,ref flag);
                    break;
                }
                case 13:
                {
                    SUB(param,ref regs,ref flag);
                    break;
                }
                case 14:
                {
                    XOR(param,ref regs,ref flag);
                    break;
                }
                case 15:
                {
                    OR(param,ref regs,ref flag);
                    break;
                }
                case 16:
                {
                    IN(param,ref regs,encryptedFile,ref fileIndex,ref ieof_flag);
                    break;
                }
                case 17:
                {
                    OUT(param,ref regs,fileName);
                    break;
                }
            }
            //Console.Write(Convert.ToString(code,16).PadLeft(2,'0')+" ");
        }

        static void MOVC(byte param,ref byte[] regs)
        {
            regs[0]=param;
        }

        static void IN(byte param,ref byte[] regs,string encryptedFileName,ref int fileIndex,ref bool ieof_flag)
        {
            //Skaito viena baita is duomenu failo.
            if(fileIndex>=encryptedFileName.Length)
            {
                ieof_flag=true;
            }
            else
            {
                regs[param]=Convert.ToByte(encryptedFileName[fileIndex]);
                fileIndex++;
            }
        }

        static void JFE(byte param,byte[] regs,bool ieof_flag,ref int programCounter)
        {
            if(ieof_flag==true)
            {
                programCounter=((programCounter+param)%256)-2;
            }
        }

        static void SUB(byte param,ref byte[] regs,ref bool flag)
        {
            int Rx=(param & 0xf);
            int Ry=(param >> 4);
            regs[Rx]-=regs[Ry];

            if(regs[Rx]==0) flag=true;
            else flag=false;
        }

        static void LSL(byte param,ref byte[] regs,ref bool flag)
        {
            int Rx=(param & 0xf);
            regs[Rx]=Convert.ToByte(regs[Rx]<<1);

            if(regs[Rx]==0) flag=true;
            else flag=false;
        }

        static void OR(byte param,ref byte[] regs,ref bool flag)
        {
            int Rx=(param & 0xf);
            int Ry=(param >> 4);
            regs[Rx]=Convert.ToByte(regs[Rx]|regs[Ry]);

            if(regs[Rx]==0) flag=true;
            else flag=false;
        }

        static void XOR(byte param,ref byte[] regs,ref bool flag)
        {
            int Rx=(param & 0xf);
            int Ry=(param >> 4);
            regs[Rx]=Convert.ToByte(regs[Rx]^regs[Ry]);

            if(regs[Rx]==0) flag=true;
            else flag=false;
        }

        static void OUT(byte param, ref byte[] regs,string fileName)
        {
            using(StreamWriter sw = File.AppendText(fileName))
            {
                sw.Write(Convert.ToChar(regs[param]));
            }
            Console.Write(Convert.ToChar(regs[param]));
        }

        static void JMP(byte param, byte[] regs,ref int programCounter)
        {
            programCounter=((programCounter+param)%256)-2;
        }

        static void ADD(byte param,ref byte[] regs,ref bool flag)
        {
            int Rx=(param & 0xf);
            int Ry=(param >> 4);
            regs[Rx]+=regs[Ry];

            if(regs[Rx]==0) flag=true;
            else flag=false;
        }

        static void INC(byte param, ref byte[] regs,ref bool flag)
        {
            int Rx=(param & 0xf);
            regs[Rx]++;

            if(regs[Rx]==0) flag=true;
            else flag=false;
        }

        static void DEC(byte param, ref byte[] regs,ref bool flag)
        {
            int Rx=(param & 0xf);
            regs[Rx]--;

            if(regs[Rx]==0) flag=true;
            else flag=false;
        }

        static void MOV(byte param, ref byte[] regs)
        {
            int Rx=(param & 0xf);
            int Ry=(param >> 4);
            regs[Rx]=regs[Ry];
        }
        
        static void LSR(byte param,ref byte[] regs,ref bool flag)
        {
            int Rx=(param & 0xf);
            regs[Rx]=Convert.ToByte(regs[Rx]>>1);

            if(regs[Rx]==0) flag=true;
            else flag=false;
        }

        static void JZ(byte param, byte[] regs,bool flag,ref int programCounter)
        {
            if(flag==true)
            {
                programCounter=((programCounter+param)%256)-2;
            }
        }

        static void JNZ(byte param, byte[] regs,bool flag,ref int programCounter)
        {
            if(flag==false)
            {
                programCounter=((programCounter+param)%256)-2;
            }
        }
    }
}
