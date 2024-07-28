

using SkillBridge.Message;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIJoinGuildItem : MonoBehaviour,IPointerClickHandler{
	public Sprite selectbg;
	public Image BG;
	public Text guildname;
    public Text guildid;
    public Text guildowner;
    public Text guildmembercount;
	UIJoinGuild owner;
    private Sprite NormalBG;
    public NGuildInfo guild;
    // Use this for initialization
    void Start () {
        NormalBG = BG.overrideSprite;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Set(string guildname,string guildid,string guildowner,string guildmembercount,UIJoinGuild owner,NGuildInfo guild) 
	{
        this.guildname.text = guildname;
		this.guildid.text = guildid;
		this.guildowner.text = guildowner;
		this.guildmembercount.text = guildmembercount;
		this.owner = owner;
        this.guild = guild;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (owner.selectItem != null)
        {
            owner.selectItem.BG.overrideSprite = NormalBG;
        }
        owner.selectItem = this;
        this.BG.overrideSprite = selectbg;
        owner.guildInfo.SetActive(true);
        owner.Set(guildname.text, guildid.text, guildmembercount.text, guild.Notice, guildowner.text);
    }
}
