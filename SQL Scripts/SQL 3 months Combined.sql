(
SELECT hd_ticket.OWNER_ID AS owner, user.USER_NAME, COUNT(hd_ticket.ID) AS total_opened, NULL AS total_closed, MONTH(hd_ticket.CREATED) AS month, YEAR(hd_ticket.CREATED) AS year
FROM HD_TICKET
JOIN HD_STATUS ON (HD_STATUS.ID = hd_ticket.HD_STATUS_ID)
LEFT JOIN USER ON user.ID=hd_ticket.OWNER_ID
WHERE (hd_ticket.HD_QUEUE_ID = 4) AND (
	(HD_STATUS.NAME NOT LIKE '%Spam%') AND (HD_STATUS.NAME NOT LIKE '%Server Status Report%')
) AND (hd_ticket.CREATED >= DATE_SUB(DATE_ADD(LAST_DAY(NOW()), INTERVAL 1 DAY), INTERVAL 3 MONTH))
GROUP BY hd_ticket.OWNER_ID, YEAR(hd_ticket.CREATED), MONTH(hd_ticket.CREATED)
ORDER BY YEAR(hd_ticket.CREATED), MONTH(hd_ticket.CREATED), hd_ticket.OWNER_ID
) UNION ALL (
SELECT hd_ticket.OWNER_ID AS owner,user.USER_NAME, NULL, COUNT(hd_ticket.ID) AS total_closed, MONTH(hd_ticket.TIME_CLOSED) AS MONTH, YEAR(hd_ticket.TIME_CLOSED) AS YEAR
FROM HD_TICKET
JOIN HD_STATUS ON (HD_STATUS.ID = hd_ticket.HD_STATUS_ID)
LEFT JOIN USER ON user.ID=hd_ticket.OWNER_ID
WHERE (hd_ticket.HD_QUEUE_ID = 4) AND (
		(HD_STATUS.STATE LIKE '%closed%') AND ((HD_STATUS.NAME NOT LIKE '%Spam%') AND (HD_STATUS.NAME NOT LIKE '%Server Status Report%'))
	) AND hd_ticket.TIME_CLOSED >= DATE_SUB(DATE_ADD(LAST_DAY(NOW()), INTERVAL 1 DAY), INTERVAL 3 MONTH)
GROUP BY hd_ticket.OWNER_ID, YEAR(hd_ticket.TIME_CLOSED), MONTH(hd_ticket.TIME_CLOSED)
ORDER BY hd_ticket.TIME_CLOSED 
)