/* 
 * Peril Beasts Copyright (C) Patrick Brisbine - All Rights Reserved
 *
 * Peril Beasts is licensed under a Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
 *
 * You should have received a copy of the license along with this
 * work.  If not, see <http://creativecommons.org/licenses/by-nc-nd/3.0/>.
 *  
 * Written by Patrick Brisbine December 2017
 */

using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject GameObjectToSpawn;

    void Start()
    {
        var newGameObject = Instantiate(GameObjectToSpawn, transform.position, transform.rotation);
        newGameObject.transform.parent = gameObject.transform;
    }
}
