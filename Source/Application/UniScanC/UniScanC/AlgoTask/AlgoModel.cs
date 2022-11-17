using System.Collections.Generic;
using System.Linq;
using UniScanC.Algorithm;
using UniScanC.Algorithm.Base;

namespace UniScanC.AlgoTask
{
    public class AlgoModel
    {
        public INodeParam AlgoParam { get; set; } = null;
        public List<ILink> AlgoLinkSList { get; set; } = new List<ILink>();

        public AlgoModel(INodeParam algoParam, List<ILink> linkList)
        {
            AlgoParam = algoParam;

            if (linkList != null)
            {
                var linksList = linkList.Cast<LinkS>().ToList();
                var filteredLinksList = linksList.FindAll(x => x.DstUnitName == algoParam.Name).ToList();
                if (!AlgoParam.GetType().Name.Equals(typeof(SetNodeParam<object>).Name))
                {
                    foreach (string inProp in AlgoParam.InPropNameTypes.Select(f => f.Item1))
                    {
                        LinkS findLink = filteredLinksList.Find(x => x.DstPortName == inProp);
                        if (findLink != null)
                        {
                            AlgoLinkSList.Add(findLink.Clone() as LinkS);
                        }
                        else
                        {
                            AlgoLinkSList.Add(new LinkS("", "", AlgoParam.Name, inProp));
                        }
                    }
                }
                else
                {
                    if (filteredLinksList != null)
                    {
                        foreach (LinkS link in filteredLinksList)
                        {
                            AlgoLinkSList.Add(link.Clone() as LinkS);
                        }
                    }
                    else
                    {
                        AlgoLinkSList.Add(new LinkS("", "", AlgoParam.Name, "ArrayList"));
                    }
                }
            }
            else
            {
                foreach (string inProp in AlgoParam.InPropNameTypes.Select(f => f.Item1))
                {
                    AlgoLinkSList.Add(new LinkS("", "", AlgoParam.Name, inProp));
                }
            }
        }

        public AlgoModel Clone()
        {
            return new AlgoModel(AlgoParam.Clone(), AlgoLinkSList.Cast<ILink>().ToList());
        }
    }
}