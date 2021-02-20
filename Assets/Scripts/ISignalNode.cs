using System.Collections.Generic;

public interface ISignalNode {
  double GetValue(double sample, Stack<ISignalNode> nodes);
  double[] GetValues(double sample, int count, Stack<ISignalNode> nodes);
}
