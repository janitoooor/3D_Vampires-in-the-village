using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IHoverObject
{
    public void ShowInteract();

    public void HideInteract();

    public void ShowSelectionIndicator();

    public void HideSelectionIndicator();
}
