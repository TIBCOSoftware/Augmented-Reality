using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TIBCO.LABS.EFTL {
  public interface IDataHandler {
    void OnData (JsonObject message);
    void Publish(JsonObject message);
    }
}
