// Copyright 2004-2007 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.Facilities.Remoting.Tests
{
	using System;
	using System.Runtime.Remoting;

	using Castle.Windsor;
	using Castle.Facilities.Remoting.TestComponents;

	using NUnit.Framework;

	[TestFixture, Serializable]
	public class RemoteComponentWithInterceptorTestCase : AbstractRemoteTestCase
	{
		protected override String GetServerConfigFile()
		{
			return BuildConfigPath("server_kernelcomponent_inter1.xml");
		}

		[Test]
		public void ClientContainerConsumingRemoteComponent()
		{
			clientDomain.DoCallBack(new CrossAppDomainDelegate(ClientContainerConsumingRemoteComponentCallback));
		}

		public void ClientContainerConsumingRemoteComponentCallback()
		{
			IWindsorContainer clientContainer = CreateRemoteContainer(clientDomain, BuildConfigPath("client_kernelcomponent.xml"));

			ICalcService service = (ICalcService) clientContainer[ typeof(ICalcService) ];

			Assert.IsTrue( RemotingServices.IsTransparentProxy(service) );
			Assert.IsTrue( RemotingServices.IsObjectOutOfAppDomain(service) );

			// The result is being intercepted, that's why we 
			// assert for 20 instead of 10
			Assert.AreEqual(20, service.Sum(7,3));
		}
	}
}
