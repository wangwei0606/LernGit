using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public delegate void CompleteHandler(int id);
public delegate void EveryHandler(int id ,int time);
public delegate void FixFrameHandler(float delta);
