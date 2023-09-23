using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DNameHelper
{
    private string m_absPath;
    private IDevelopNameRule m_rule;
    private NameRuleCfg m_cfg;
    public DNameHelper(NameRuleCfg cfg)
    {
        m_cfg = cfg;
        m_rule = DNameRuleMgr.Instance.getRule(cfg.ruleId);
    }

    public DNameHelper(string absPath,int ruleId)
    {
        m_absPath = absPath;
        m_rule = DNameRuleMgr.Instance.getRule(ruleId);
    }

    public void setName(string rootPath)
    {
        if(m_rule!=null)
        {
            m_rule.setBundleName(rootPath, m_cfg.absPath, m_cfg.abPath);
        }
    }

    public void clearName(string rootPath)
    {
        if(m_rule!=null)
        {
            m_rule.clearBundleName(rootPath, m_cfg.absPath);
        }
    }
}