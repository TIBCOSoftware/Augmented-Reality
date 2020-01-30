using System;

namespace TIBCO.LABS.EFTL {
  public interface IDataReceiver {
    void OnReceivedData (object sender, EventArgs args);
  }
}
