﻿/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Hubble.Framework.DataStructure;
using Hubble.Core.SFQL.LexicalAnalysis;

namespace Hubble.Core.SFQL.SyntaxAnalysis
{
    abstract public class Syntax<Function> : DFA<LexicalAnalysis.Lexical.Token, Function> 
    {
        public static SyntaxState<Function> AddSyntaxState(SyntaxState<Function> state)
        {
            return (SyntaxState<Function>)AddState((DFAState<Lexical.Token, Function>)state);
        }

        new public virtual DFAResult Input(int action, LexicalAnalysis.Lexical.Token token)
        {
            try
            {
                return base.Input(action, token);
            }
            catch (DFAException dfaEx)
            {
                throw new SyntaxException(string.Format("{0}, current class:{1}", dfaEx.Message, this.GetType()), this, dfaEx, token);
            }
        }
    }
}
