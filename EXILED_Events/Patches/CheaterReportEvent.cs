using Harmony;
using System;

namespace EXILED.Patches
{
	[HarmonyPatch(typeof(CheaterReport), nameof(CheaterReport.IssueReport))]
	public class CheaterReportEvent
	{
		public static bool Prefix(CheaterReport __instance, GameConsoleTransmission reporter,
			string reporterUserId,
			string reportedUserId,
			string reportedAuth,
			string reportedIp,
			string reporterAuth,
			string reporterIp,
			ref string reason,
			ref byte[] signature,
			string reporterPublicKey,
			int reportedId)
		{
			if (EventPlugin.CheaterReportPatchDisable)
				return true;

			try
			{
				if (reportedAuth == reporterAuth)
					reporter.SendToClient(__instance.connectionToClient,
						"You can't report yourself!" + Environment.NewLine, "yellow");

				int serverId = ServerConsole.Port;
				bool allow = true;

				Events.InvokeCheaterReport(reporterAuth, reportedAuth, reportedIp, reason, serverId, ref allow);

				return allow;
			}
			catch (Exception exception)
			{
				Log.Error($"CheaterReportEvent error: {exception}");
				return true;
			}
		}
	}
}