using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCameraBackUp : MonoBehaviour
{
    //private PlayerMaskChange playerMaskChange;
    //private PlayerMovement playerMovement;
    //private PlayerSkillSet playerSkillSet;

    //[SerializeField] private PlayerData playerData;

    //[SerializeField] private Camera mainCamera;

    //[SerializeField] private Transform humanCameraTarget;
    //[SerializeField] private Transform animalCameraTarget;

    //[SerializeField] private Collider currentTarget;
    //public Collider CurrentTarget { get { return currentTarget; } }

    //private void Awake()
    //{
    //    playerMaskChange = GetComponent<PlayerMaskChange>();
    //    playerMovement = GetComponent<PlayerMovement>();
    //    playerSkillSet = GetComponent<PlayerSkillSet>();
    //    mainCamera = GetComponent<Camera>();
    //}

    //public void CameraSetting()
    //{
    //    freeLookCamera.Follow = playerMaskChange.ActiveCharacter.transform;

    //    if (playerMaskChange.ActiveCharacter.name == "HumanMaskCharacter")
    //    {
    //        freeLookCamera.LookAt = humanCameraTarget;
    //    }
    //    else
    //    {
    //        freeLookCamera.LookAt = animalCameraTarget;
    //    }
    //}
    //public void LeftDirection()
    //{
    //    freeLookCamera.m_XAxis.Value -= playerData.cameraYAngle;
    //}
    //public void RightDirection()
    //{
    //    freeLookCamera.m_XAxis.Value += playerData.cameraYAngle;
    //}

    //public void DetectTarget()
    //{
    //    if (currentTarget) //�̹� ���� ���̸� ���� ����
    //    {
    //        currentTarget = null;
    //        if (playerMaskChange.ActiveCharacter.name == "HumanMaskCharacter")
    //        {
    //            freeLookCamera.LookAt = humanCameraTarget.transform;
    //        }
    //        else
    //        {
    //            freeLookCamera.LookAt = animalCameraTarget.transform;
    //        }
    //    }
    //    else //������ �ƴϸ� Ž��
    //    {
    //        Collider[] colls = Physics.OverlapSphere(playerMaskChange.ActiveCharacter.transform.position, playerData.detectRange, playerData.targetLayer);

    //        if (colls.Length != 0 && colls != null)
    //        {
    //            float smallestAngle = Mathf.Infinity;

    //            for (int i = 0; i < colls.Length; i++)
    //            {
    //                Collider detectedTarget = colls[i];
    //                Vector3 directionTowardTarget = detectedTarget.transform.position - freeLookCamera.transform.position;
    //                float angleWithTarget = Vector3.Angle(directionTowardTarget, freeLookCamera.transform.forward);
    //                float distanceFromTarget = Vector3.Distance(playerMaskChange.ActiveCharacter.transform.position, detectedTarget.transform.position);

    //                if (distanceFromTarget > playerData.detectRange) continue;
    //                if (angleWithTarget > playerData.maximumAngleWithTarget) continue;
    //                //��ֹ��� �ִٸ� continue;

    //                if (angleWithTarget < smallestAngle)
    //                {
    //                    smallestAngle = angleWithTarget;
    //                    currentTarget = detectedTarget;
    //                }
    //            }
    //            if (currentTarget) LockOnTarget();
    //            return;
    //        }
    //        //Ž���� �ȵǸ� ���� �ٶ󺸱�
    //        AlignCameraToPlayerForward();
    //    }
    //}

    //public void AlignCameraToPlayerForward()
    //{
    //    //ĳ���� yȸ�� ���� freelook x axis value ���� ��ġ
    //    //while (playerMovement.MoveAmount < .1f && !playerSkillSet.RestrictForSkill 
    //    //    && Mathf.Abs( freeLookCamera.m_XAxis.Value - playerMaskChange.ActiveCharacter.transform.eulerAngles.y) > 10) 
    //    //{
    //    //    freeLookCamera.m_XAxis.Value = Mathf.Lerp(freeLookCamera.m_XAxis.Value, playerMaskChange.ActiveCharacter.transform.eulerAngles.y, playerData.alignCameraSmoothTime);
    //    //    yield return new WaitForSeconds(.01f);
    //    //}

    //    freeLookCamera.m_XAxis.Value = playerMaskChange.ActiveCharacter.transform.eulerAngles.y;
    //}

    //public void LockOnTarget()
    //{
    //    freeLookCamera.LookAt = currentTarget.gameObject.transform;
    //}

    ////������ x axis value�� ������
    ////������ x axis value�� �����
}
